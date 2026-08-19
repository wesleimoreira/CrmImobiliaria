using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Lead : AggregateRoot
    {
        private static readonly EstagioFunil[] Sequencia =
        [
            EstagioFunil.NovoLead, EstagioFunil.Contato, EstagioFunil.Qualificado,
            EstagioFunil.ImovelApresentado, EstagioFunil.Visita, EstagioFunil.Proposta,
            EstagioFunil.Negociacao, EstagioFunil.Fechado
        ];

        private readonly List<RegistroEstagio> _historico = [];
        private readonly List<Guid> _imoveisApresentados = [];

        public Guid ClienteId { get; private set; }
        public Guid CorretorId { get; private set; }
        public EstagioFunil EstagioAtual { get; private set; }
        public EstagioFunil? EstagioAntesDePerda { get; private set; }
        public string? MotivoPerda { get; private set; }
        public IReadOnlyList<RegistroEstagio> Historico => _historico;
        public IReadOnlyList<Guid> ImoveisApresentados => _imoveisApresentados;

        private Lead(Guid clienteId, Guid corretorId)
        {
            ClienteId = clienteId;
            CorretorId = corretorId;
            EstagioAtual = EstagioFunil.NovoLead;
            _historico.Add(new RegistroEstagio(EstagioFunil.NovoLead, DateTime.UtcNow));
        }

        private Lead() { }

        public static Result<Lead> Criar(Guid clienteId, Guid corretorId) => Result<Lead>.Success(new Lead(clienteId, corretorId));

        private Result<bool> AvancarPara(EstagioFunil destino, string? observacao = null)
        {
            var indiceAtual = Array.IndexOf(Sequencia, EstagioAtual);
            var indiceDestino = Array.IndexOf(Sequencia, destino);

            if (indiceAtual < 0)
                return Result<bool>.Failure($"Não é possível avançar a partir do estágio {EstagioAtual}.");

            if (indiceDestino <= indiceAtual)
                return Result<bool>.Failure($"{destino} não é um avanço válido a partir de {EstagioAtual}.");

            EstagioAtual = destino;
            _historico.Add(new RegistroEstagio(destino, DateTime.UtcNow, observacao));
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RegistrarContato(string? observacao = null) => AvancarPara(EstagioFunil.Contato, observacao);
        public Result<bool> Qualificar(string? observacao = null) => AvancarPara(EstagioFunil.Qualificado, observacao);
        public Result<bool> RegistrarVisita(string? observacao = null) => AvancarPara(EstagioFunil.Visita, observacao);
        public Result<bool> RegistrarProposta(string? observacao = null) => AvancarPara(EstagioFunil.Proposta, observacao);
        public Result<bool> IniciarNegociacao(string? observacao = null) => AvancarPara(EstagioFunil.Negociacao, observacao);
        public Result<bool> Fechar(string? observacao = null) => AvancarPara(EstagioFunil.Fechado, observacao);

        public Result<bool> ApresentarImovel(Guid anuncioImovelId)
        {
            if (EstagioAtual is EstagioFunil.Fechado or EstagioFunil.Perdido)
                return Result<bool>.Failure($"Não é possível apresentar imóvel com o lead {EstagioAtual}.");

            _imoveisApresentados.Add(anuncioImovelId);

            var indiceAtual = Array.IndexOf(Sequencia, EstagioAtual);
            var indiceAlvo = Array.IndexOf(Sequencia, EstagioFunil.ImovelApresentado);

            if (indiceAtual < indiceAlvo)
                return AvancarPara(EstagioFunil.ImovelApresentado);

            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> MarcarPerdido(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo da perda é obrigatório.");

            if (EstagioAtual is EstagioFunil.Fechado or EstagioFunil.Perdido)
                return Result<bool>.Failure($"Não é possível marcar como perdido um lead {EstagioAtual}.");

            EstagioAntesDePerda = EstagioAtual;
            MotivoPerda = motivo;
            EstagioAtual = EstagioFunil.Perdido;
            _historico.Add(new RegistroEstagio(EstagioFunil.Perdido, DateTime.UtcNow, motivo));
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Reabrir()
        {
            if (EstagioAtual != EstagioFunil.Perdido)
                return Result<bool>.Failure("Só é possível reabrir um lead perdido.");

            EstagioAtual = EstagioAntesDePerda ?? EstagioFunil.NovoLead;
            MotivoPerda = null;
            EstagioAntesDePerda = null;
            _historico.Add(new RegistroEstagio(EstagioAtual, DateTime.UtcNow, "Lead reaberto"));
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> TransferirCorretor(Guid novoCorretorId)
        {
            CorretorId = novoCorretorId;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}

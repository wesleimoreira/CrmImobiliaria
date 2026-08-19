using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.Events;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Locacao : AggregateRoot
    {
        private static readonly EstagioLocacao[] Sequencia =
        [
            EstagioLocacao.Lead, EstagioLocacao.Visita, EstagioLocacao.AnaliseCadastral,
        EstagioLocacao.Aprovacao, EstagioLocacao.Contrato, EstagioLocacao.Vistoria,
        EstagioLocacao.EntregaChaves
        ];

        public Guid ProprietarioId { get; private set; }
        public Guid LocatarioId { get; private set; }
        public Guid ImovelId { get; private set; }
        public Guid AnuncioImovelId { get; private set; }
        public Guid CorretorId { get; private set; }
        public Guid? LeadId { get; private set; }
        public EstagioLocacao EstagioAtual { get; private set; }
        public StatusLocacao Status { get; private set; }
        public Dinheiro? ValorAluguel { get; private set; }        // definido quando chega em Contrato
        public PeriodoContrato? Periodo { get; private set; }
        public int? DiaVencimento { get; private set; }
        public Dinheiro? Garantia { get; private set; }
        public Percentual? TaxaAdministracao { get; private set; }
        public IndiceReajuste? IndiceReajuste { get; private set; }
        public string? MotivoReprovacaoOuCancelamento { get; private set; }

        private Locacao(Guid proprietarioId, Guid locatarioId, Guid imovelId, Guid anuncioImovelId, Guid corretorId, Guid? leadId)
        {
            ProprietarioId = proprietarioId;
            LocatarioId = locatarioId;
            ImovelId = imovelId;
            AnuncioImovelId = anuncioImovelId;
            CorretorId = corretorId;
            LeadId = leadId;
            EstagioAtual = EstagioLocacao.Lead;
            Status = StatusLocacao.EmAndamento;
        }

        private Locacao() { }

        public static Result<Locacao> Iniciar(Guid proprietarioId, Guid locatarioId, Guid imovelId, Guid anuncioImovelId, Guid corretorId, Guid? leadId = null) => Result<Locacao>.Success(new Locacao(proprietarioId, locatarioId, imovelId, anuncioImovelId, corretorId, leadId));

        private Result<bool> AvancarPara(EstagioLocacao destino)
        {
            if (Status != StatusLocacao.EmAndamento)
                return Result<bool>.Failure($"Não é possível avançar uma locação com status {Status}.");

            var indiceAtual = Array.IndexOf(Sequencia, EstagioAtual);
            var indiceDestino = Array.IndexOf(Sequencia, destino);

            if (indiceDestino <= indiceAtual)
                return Result<bool>.Failure($"{destino} não é um avanço válido a partir de {EstagioAtual}.");

            EstagioAtual = destino;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RegistrarVisita() => AvancarPara(EstagioLocacao.Visita);
        public Result<bool> IniciarAnaliseCadastral() => AvancarPara(EstagioLocacao.AnaliseCadastral);

        public Result<bool> Aprovar()
        {
            var resultado = AvancarPara(EstagioLocacao.Aprovacao);
            return resultado;
        }

        public Result<bool> Reprovar(string motivo)
        {
            if (EstagioAtual != EstagioLocacao.AnaliseCadastral)
                return Result<bool>.Failure("Só é possível reprovar durante a análise cadastral.");

            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo da reprovação é obrigatório.");

            Status = StatusLocacao.Reprovada;
            MotivoReprovacaoOuCancelamento = motivo;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> FormalizarContrato(Dinheiro valorAluguel, PeriodoContrato periodo, int diaVencimento, Dinheiro garantia, Percentual taxaAdministracao, IndiceReajuste indiceReajuste)
        {
            if (EstagioAtual != EstagioLocacao.Aprovacao)
                return Result<bool>.Failure("Contrato só pode ser formalizado após aprovação.");

            if (diaVencimento is < 1 or > 31)
                return Result<bool>.Failure("Dia de vencimento deve estar entre 1 e 31.");

            var resultado = AvancarPara(EstagioLocacao.Contrato);

            if (!resultado.IsSuccess)
                return resultado;

            ValorAluguel = valorAluguel;
            Periodo = periodo;
            DiaVencimento = diaVencimento;
            Garantia = garantia;
            TaxaAdministracao = taxaAdministracao;
            IndiceReajuste = indiceReajuste;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> RegistrarVistoriaEntrada() => AvancarPara(EstagioLocacao.Vistoria);

        public Result<bool> EntregarChaves()
        {
            var resultado = AvancarPara(EstagioLocacao.EntregaChaves);

            if (!resultado.IsSuccess)
                return resultado;

            Status = StatusLocacao.Ativa;
            MarcarAtualizado();
            RegistrarEvento(new LocacaoChavesEntreguesEvent(Id, AnuncioImovelId, DateTime.UtcNow));
            return Result<bool>.Success(true);
        }

        public Result<bool> Encerrar(string? motivo = null)
        {
            if (Status != StatusLocacao.Ativa)
                return Result<bool>.Failure("Só é possível encerrar uma locação ativa.");

            Status = StatusLocacao.Encerrada;
            MotivoReprovacaoOuCancelamento = motivo;
            MarcarAtualizado();
            RegistrarEvento(new LocacaoEncerradaEvent(Id, AnuncioImovelId, DateTime.UtcNow));
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar(string motivo)
        {
            if (Status is StatusLocacao.Ativa or StatusLocacao.Encerrada)
                return Result<bool>.Failure($"Não é possível cancelar uma locação {Status}.");

            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo do cancelamento é obrigatório.");

            Status = StatusLocacao.Cancelada;
            MotivoReprovacaoOuCancelamento = motivo;
            MarcarAtualizado();
            RegistrarEvento(new LocacaoCanceladaEvent(Id, AnuncioImovelId, DateTime.UtcNow));
            return Result<bool>.Success(true);
        }

        public DateOnly? ProximaDataReajuste(DateOnly referencia)
        {
            if (Periodo is null) return null;

            var proxima = Periodo.DataInicial.AddYears(1);

            while (proxima <= referencia)
                proxima = proxima.AddYears(1);

            return proxima;
        }
    }
}

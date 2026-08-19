using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Proposta : AggregateRoot
    {
        private readonly List<RegistroNegociacao> _historico = [];

        public Guid ClienteId { get; private set; }
        public Guid? ImovelId { get; private set; }
        public Guid? AnuncioImovelId { get; private set; }
        public Guid? LoteId { get; private set; }
        public Guid CorretorId { get; private set; }
        public Guid? LeadId { get; private set; }
        public Dinheiro ValorAnunciado { get; private set; }
        public Dinheiro ValorProposto { get; private set; }
        public Dinheiro? Entrada { get; private set; }
        public int? NumeroParcelas { get; private set; }
        public Dinheiro? ValorParcela { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }
        public StatusProposta Status { get; private set; }
        public string? MotivoRecusa { get; private set; }
        public IReadOnlyList<RegistroNegociacao> Historico => _historico;

        public bool EhParaImovel => ImovelId is not null;
        public bool EhParaLote => LoteId is not null;
        public Dinheiro UltimoValorNegociado => _historico[^1].Valor;

        private Proposta(Guid clienteId, Guid? imovelId, Guid? anuncioImovelId, Guid? loteId, Guid corretorId, Guid? leadId, Dinheiro valorAnunciado, Dinheiro valorProposto, Dinheiro? entrada, int? numeroParcelas, Dinheiro? valorParcela, FormaPagamento formaPagamento)
        {
            ClienteId = clienteId;
            ImovelId = imovelId;
            AnuncioImovelId = anuncioImovelId;
            LoteId = loteId;
            CorretorId = corretorId;
            LeadId = leadId;
            ValorAnunciado = valorAnunciado;
            ValorProposto = valorProposto;
            Entrada = entrada;
            NumeroParcelas = numeroParcelas;
            ValorParcela = valorParcela;
            FormaPagamento = formaPagamento;
            Status = StatusProposta.EmAnalise;
            _historico.Add(new RegistroNegociacao(valorProposto, OrigemNegociacao.Cliente, DateTime.UtcNow));
        }

        private Proposta()
        {
            ValorAnunciado = null!;
            ValorProposto = null!;
        }

        private static Result<Proposta> Criar(Guid clienteId, Guid? imovelId, Guid? anuncioImovelId, Guid? loteId, Guid corretorId, Dinheiro valorAnunciado, Dinheiro valorProposto, FormaPagamento formaPagamento, Guid? leadId, Dinheiro? entrada, int? numeroParcelas, Dinheiro? valorParcela)
        {
            var ehImovel = imovelId is not null;
            var ehLote = loteId is not null;

            if (ehImovel == ehLote)
                return Result<Proposta>.Failure("Proposta deve ser de um imóvel OU um lote, não ambos nem nenhum.");

            if (ehImovel && anuncioImovelId is null)
                return Result<Proposta>.Failure("Anúncio do imóvel é obrigatório para proposta de imóvel.");

            if (numeroParcelas is <= 0)
                return Result<Proposta>.Failure("Número de parcelas deve ser maior que zero.");

            if ((numeroParcelas is null) != (valorParcela is null))
                return Result<Proposta>.Failure("Número de parcelas e valor da parcela devem ser informados juntos.");

            if (formaPagamento == FormaPagamento.AVista && numeroParcelas is not null)
                return Result<Proposta>.Failure("Pagamento à vista não deve ter parcelamento.");

            return Result<Proposta>.Success(new Proposta(clienteId, imovelId, anuncioImovelId, loteId, corretorId, leadId, valorAnunciado, valorProposto, entrada, numeroParcelas, valorParcela, formaPagamento));
        }

        public static Result<Proposta> ParaImovel(
            Guid clienteId, Guid imovelId, Guid anuncioImovelId, Guid corretorId,
            Dinheiro valorAnunciado, Dinheiro valorProposto, FormaPagamento formaPagamento,
            Guid? leadId = null, Dinheiro? entrada = null, int? numeroParcelas = null, Dinheiro? valorParcela = null) =>
            Criar(clienteId, imovelId, anuncioImovelId, null, corretorId, valorAnunciado, valorProposto, formaPagamento, leadId, entrada, numeroParcelas, valorParcela);

        public static Result<Proposta> ParaLote(
            Guid clienteId, Guid loteId, Guid corretorId,
            Dinheiro valorAnunciado, Dinheiro valorProposto, FormaPagamento formaPagamento,
            Guid? leadId = null, Dinheiro? entrada = null, int? numeroParcelas = null, Dinheiro? valorParcela = null) =>
            Criar(clienteId, null, null, loteId, corretorId, valorAnunciado, valorProposto, formaPagamento, leadId, entrada, numeroParcelas, valorParcela);

        public Result<bool> RegistrarContraproposta(Dinheiro valor, OrigemNegociacao origem, string? observacao = null)
        {
            if (Status is StatusProposta.Aceita or StatusProposta.Recusada or StatusProposta.Cancelada)
                return Result<bool>.Failure($"Não é possível negociar uma proposta {Status}.");

            _historico.Add(new RegistroNegociacao(valor, origem, DateTime.UtcNow, observacao));
            Status = StatusProposta.ContraProposta;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Aceitar()
        {
            if (Status is not (StatusProposta.EmAnalise or StatusProposta.ContraProposta))
                return Result<bool>.Failure($"Não é possível aceitar uma proposta {Status}.");

            Status = StatusProposta.Aceita;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Recusar(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo da recusa é obrigatório.");

            if (Status is StatusProposta.Aceita or StatusProposta.Recusada or StatusProposta.Cancelada)
                return Result<bool>.Failure($"Não é possível recusar uma proposta {Status}.");

            Status = StatusProposta.Recusada;
            MotivoRecusa = motivo;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Cancelar()
        {
            if (Status is StatusProposta.Aceita)
                return Result<bool>.Failure("Não é possível cancelar uma proposta já aceita.");

            Status = StatusProposta.Cancelada;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }
    }
}

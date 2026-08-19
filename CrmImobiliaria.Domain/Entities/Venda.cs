using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.Events;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class Venda : AggregateRoot
    {
        public Guid PropostaId { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? ProprietarioId { get; private set; }     // aplica-se apenas à venda de imóvel
        public Guid? ImovelId { get; private set; }
        public Guid? AnuncioImovelId { get; private set; }
        public Guid? LoteId { get; private set; }
        public Guid CorretorId { get; private set; }
        public Dinheiro ValorFinal { get; private set; }
        public DateOnly DataVenda { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }
        public DadosFinanciamento? Financiamento { get; private set; }
        public SituacaoDocumental SituacaoDocumental { get; private set; }
        public string? NumeroContrato { get; private set; }
        public string? UrlContrato { get; private set; }
        public StatusVenda Status { get; private set; }
        public string? MotivoDistrato { get; private set; }

        public bool EhParaImovel => ImovelId is not null;
        public bool EhParaLote => LoteId is not null;

        private Venda(Guid propostaId, Guid clienteId, Guid? proprietarioId, Guid? imovelId, Guid? anuncioImovelId, Guid? loteId, Guid corretorId, Dinheiro valorFinal, DateOnly dataVenda, FormaPagamento formaPagamento, DadosFinanciamento? financiamento)
        {
            PropostaId = propostaId;
            ClienteId = clienteId;
            ProprietarioId = proprietarioId;
            ImovelId = imovelId;
            AnuncioImovelId = anuncioImovelId;
            LoteId = loteId;
            CorretorId = corretorId;
            ValorFinal = valorFinal;
            DataVenda = dataVenda;
            FormaPagamento = formaPagamento;
            Financiamento = financiamento;
            SituacaoDocumental = SituacaoDocumental.Pendente;
            Status = StatusVenda.Concluida;

            RegistrarEvento(new VendaConcluidaEvent(Id, ImovelId, AnuncioImovelId, LoteId, CorretorId, ClienteId, ValorFinal, DateTime.UtcNow));
        }

        private Venda()
        {
            ValorFinal = null!;
        }

        private static Result<Venda> Fechar(Guid propostaId, Guid clienteId, Guid? proprietarioId, Guid? imovelId, Guid? anuncioImovelId, Guid? loteId, Guid corretorId, Dinheiro valorFinal, DateOnly dataVenda, FormaPagamento formaPagamento, DadosFinanciamento? financiamento)
        {
            var ehImovel = imovelId is not null;
            var ehLote = loteId is not null;

            if (ehImovel == ehLote)
                return Result<Venda>.Failure("Venda deve ser de um imóvel OU um lote, não ambos nem nenhum.");

            if (ehImovel && (anuncioImovelId is null || proprietarioId is null))
                return Result<Venda>.Failure("Anúncio e proprietário são obrigatórios para venda de imóvel.");

            if (formaPagamento == FormaPagamento.FinanciamentoBancario && financiamento is null)
                return Result<Venda>.Failure("Dados de financiamento são obrigatórios quando a forma de pagamento é financiamento bancário.");

            if (formaPagamento != FormaPagamento.FinanciamentoBancario && financiamento is not null)
                return Result<Venda>.Failure("Dados de financiamento só se aplicam à forma de pagamento financiamento bancário.");

            return Result<Venda>.Success(new Venda(propostaId, clienteId, proprietarioId, imovelId, anuncioImovelId, loteId, corretorId, valorFinal, dataVenda, formaPagamento, financiamento));
        }

        public static Result<Venda> DeImovel(
            Guid propostaId, Guid clienteId, Guid proprietarioId, Guid imovelId, Guid anuncioImovelId,
            Guid corretorId, Dinheiro valorFinal, DateOnly dataVenda, FormaPagamento formaPagamento,
            DadosFinanciamento? financiamento = null) =>
            Fechar(propostaId, clienteId, proprietarioId, imovelId, anuncioImovelId, null, corretorId, valorFinal, dataVenda, formaPagamento, financiamento);

        public static Result<Venda> DeLote(
            Guid propostaId, Guid clienteId, Guid loteId,
            Guid corretorId, Dinheiro valorFinal, DateOnly dataVenda, FormaPagamento formaPagamento,
            DadosFinanciamento? financiamento = null) =>
            Fechar(propostaId, clienteId, null, null, null, loteId, corretorId, valorFinal, dataVenda, formaPagamento, financiamento);

        public void AtualizarSituacaoDocumental(SituacaoDocumental situacao)
        {
            SituacaoDocumental = situacao;
            MarcarAtualizado();
        }

        public Result<bool> AnexarContrato(string numeroContrato, string? urlContrato = null)
        {
            if (string.IsNullOrWhiteSpace(numeroContrato))
                return Result<bool>.Failure("Número do contrato é obrigatório.");

            NumeroContrato = numeroContrato.Trim();
            UrlContrato = urlContrato;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public Result<bool> Distratar(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return Result<bool>.Failure("Motivo do distrato é obrigatório.");

            if (Status == StatusVenda.Distratada)
                return Result<bool>.Failure("Venda já está distratada.");

            Status = StatusVenda.Distratada;
            MotivoDistrato = motivo;
            MarcarAtualizado();
            RegistrarEvento(new VendaDistratadaEvent(Id, ImovelId, AnuncioImovelId, LoteId, DateTime.UtcNow));
            return Result<bool>.Success(true);
        }
    }
}

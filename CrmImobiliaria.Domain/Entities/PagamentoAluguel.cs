using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class PagamentoAluguel : AggregateRoot
    {
        public Guid LocacaoId { get; private set; }
        public Competencia Competencia { get; private set; }
        public Dinheiro ValorDevido { get; private set; }
        public DateOnly DataVencimento { get; private set; }
        public StatusPagamentoAluguel Status { get; private set; }
        public DateOnly? DataPagamento { get; private set; }
        public Dinheiro? ValorPago { get; private set; }

        private PagamentoAluguel(Guid locacaoId, Competencia competencia, Dinheiro valorDevido, DateOnly dataVencimento)
        {
            LocacaoId = locacaoId;
            Competencia = competencia;
            ValorDevido = valorDevido;
            DataVencimento = dataVencimento;
            Status = StatusPagamentoAluguel.Pendente;
        }

        private PagamentoAluguel()
        {
            Competencia = null!;
            ValorDevido = null!;
        }

        public static Result<PagamentoAluguel> Gerar(Guid locacaoId, Competencia competencia, Dinheiro valorDevido, DateOnly dataVencimento) => Result<PagamentoAluguel>.Success(new PagamentoAluguel(locacaoId, competencia, valorDevido, dataVencimento));

        public Result<bool> RegistrarPagamento(DateOnly dataPagamento, Dinheiro valorPago)
        {
            if (Status == StatusPagamentoAluguel.Pago)
                return Result<bool>.Failure("Pagamento já registrado para esta competência.");

            Status = StatusPagamentoAluguel.Pago;
            DataPagamento = dataPagamento;
            ValorPago = valorPago;
            MarcarAtualizado();
            return Result<bool>.Success(true);
        }

        public bool EstaAtrasado(DateOnly referencia) => Status == StatusPagamentoAluguel.Pendente && referencia > DataVencimento;

        public int DiasAtraso(DateOnly referencia) => EstaAtrasado(referencia) ? referencia.DayNumber - DataVencimento.DayNumber : 0;
    }
}

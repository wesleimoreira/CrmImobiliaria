using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class PeriodoContrato : ValueObject
    {
        public DateOnly DataInicial { get; }
        public DateOnly DataFinal { get; }

        private PeriodoContrato(DateOnly dataInicial, DateOnly dataFinal)
        {
            DataInicial = dataInicial;
            DataFinal = dataFinal;
        }

        public static Result<PeriodoContrato> Criar(DateOnly dataInicial, DateOnly dataFinal)
        {
            if (dataFinal <= dataInicial)
                return Result<PeriodoContrato>.Failure("Data final deve ser posterior à data inicial.");

            return Result<PeriodoContrato>.Success(new PeriodoContrato(dataInicial, dataFinal));
        }

        public int DuracaoEmDias => DataFinal.DayNumber - DataInicial.DayNumber;

        public bool EstaVigente(DateOnly referencia) => referencia >= DataInicial && referencia <= DataFinal;

        public bool EstaVencido(DateOnly referencia) => referencia > DataFinal;

        public int DiasParaVencer(DateOnly referencia) => Math.Max(0, DataFinal.DayNumber - referencia.DayNumber);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return DataInicial;
            yield return DataFinal;
        }

        public override string ToString() => $"{DataInicial:dd/MM/yyyy} a {DataFinal:dd/MM/yyyy}";
    }
}

using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Competencia : ValueObject
    {
        public int Mes { get; }
        public int Ano { get; }

        private Competencia(int mes, int ano)
        {
            Mes = mes;
            Ano = ano;
        }

        public static Result<Competencia> Criar(int mes, int ano)
        {
            if (mes is < 1 or > 12)
                return Result<Competencia>.Failure("Mês deve estar entre 1 e 12.");

            if (ano is < 2000 or > 2100)
                return Result<Competencia>.Failure("Ano inválido.");

            return Result<Competencia>.Success(new Competencia(mes, ano));
        }

        public DateOnly PrimeiroDia => new(Ano, Mes, 1);
        public DateOnly UltimoDia => PrimeiroDia.AddMonths(1).AddDays(-1);
        public Competencia Proxima() => PrimeiroDia.AddMonths(1) is var d ? Criar(d.Month, d.Year).Value! : this;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Mes;
            yield return Ano;
        }

        public override string ToString() => $"{Mes:D2}/{Ano}";
    }
}

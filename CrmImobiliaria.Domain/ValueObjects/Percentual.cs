using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Percentual : ValueObject
    {
        public decimal Valor { get; }        // ex: 5.5 significa 5,5%
        public decimal Fracao => Valor / 100m;

        private Percentual(decimal valor) => Valor = valor;

        // minimo/maximo configuráveis: índice de reajuste pode ser negativo (deflação),
        // comissão normalmente é 0-100
        public static Result<Percentual> Criar(decimal valor, decimal minimo = 0, decimal maximo = 100)
        {
            if (valor < minimo)
                return Result<Percentual>.Failure($"Percentual não pode ser menor que {minimo}%.");

            if (valor > maximo)
                return Result<Percentual>.Failure($"Percentual não pode ser maior que {maximo}%.");

            return Result<Percentual>.Success(new Percentual(Math.Round(valor, 2, MidpointRounding.AwayFromZero)));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => $"{Valor:0.##}%";
    }
}

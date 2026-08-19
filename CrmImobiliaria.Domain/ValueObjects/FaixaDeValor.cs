using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class FaixaDeValor : ValueObject
    {
        public Dinheiro Minimo { get; }
        public Dinheiro Maximo { get; }

        private FaixaDeValor(Dinheiro minimo, Dinheiro maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
        }

        public static Result<FaixaDeValor> Criar(Dinheiro minimo, Dinheiro maximo)
        {
            if (minimo.Valor < 0)
                return Result<FaixaDeValor>.Failure("Valor mínimo não pode ser negativo.");

            if (maximo.Valor < minimo.Valor)
                return Result<FaixaDeValor>.Failure("Valor máximo deve ser maior ou igual ao mínimo.");

            return Result<FaixaDeValor>.Success(new FaixaDeValor(minimo, maximo));
        }

        public bool Contem(Dinheiro valor) => valor.Valor >= Minimo.Valor && valor.Valor <= Maximo.Valor;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Minimo;
            yield return Maximo;
        }

        public override string ToString() => $"{Minimo} - {Maximo}";
    }
}

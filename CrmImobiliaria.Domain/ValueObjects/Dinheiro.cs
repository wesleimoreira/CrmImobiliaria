using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Shared;
using System.Globalization;

namespace CrmImobiliaria.Domain.ValueObjects
{
    public sealed class Dinheiro : ValueObject
    {
        public decimal Valor { get; }

        private Dinheiro(decimal valor) => Valor = valor;

        public static Dinheiro Zero => new(0);

        // Permite negativo — necessário para saldo do proprietário (aluguel - taxas - despesas)
        public static Result<Dinheiro> Criar(decimal valor) => Result<Dinheiro>.Success(new Dinheiro(Math.Round(valor, 2, MidpointRounding.AwayFromZero)));

        // Use este para valor de imóvel, venda, aluguel etc. — onde negativo/zero não faz sentido
        public static Result<Dinheiro> CriarPositivo(decimal valor)
        {
            if (valor <= 0)
                return Result<Dinheiro>.Failure("Valor deve ser maior que zero.");

            return Criar(valor);
        }

        public static Dinheiro operator +(Dinheiro a, Dinheiro b) => new(a.Valor + b.Valor);
        public static Dinheiro operator -(Dinheiro a, Dinheiro b) => new(a.Valor - b.Valor);
        public static Dinheiro operator *(Dinheiro a, Percentual p) => new(Math.Round(a.Valor * p.Fracao, 2, MidpointRounding.AwayFromZero));

        public static bool operator >(Dinheiro a, Dinheiro b) => a.Valor > b.Valor;
        public static bool operator <(Dinheiro a, Dinheiro b) => a.Valor < b.Valor;
        public static bool operator >=(Dinheiro a, Dinheiro b) => a.Valor >= b.Valor;
        public static bool operator <=(Dinheiro a, Dinheiro b) => a.Valor <= b.Valor;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor.ToString("C2", new CultureInfo("pt-BR"));
    }
}

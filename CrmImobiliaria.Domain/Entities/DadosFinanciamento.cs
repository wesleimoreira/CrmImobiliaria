using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed class DadosFinanciamento : ValueObject
    {
        public string Banco { get; }
        public Dinheiro ValorFinanciado { get; }

        private DadosFinanciamento(string banco, Dinheiro valorFinanciado)
        {
            Banco = banco;
            ValorFinanciado = valorFinanciado;
        }

        public static Result<DadosFinanciamento> Criar(string? banco, Dinheiro valorFinanciado)
        {
            if (string.IsNullOrWhiteSpace(banco))
                return Result<DadosFinanciamento>.Failure("Banco é obrigatório para venda financiada.");

            if (valorFinanciado.Valor <= 0)
                return Result<DadosFinanciamento>.Failure("Valor financiado deve ser maior que zero.");

            return Result<DadosFinanciamento>.Success(new DadosFinanciamento(banco.Trim(), valorFinanciado));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Banco;
            yield return ValorFinanciado;
        }
    }
}

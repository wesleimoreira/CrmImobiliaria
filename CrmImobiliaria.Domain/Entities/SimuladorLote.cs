using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Domain.Entities
{
    public static class SimuladorLote
    {
        public static Result<SimulacaoParcelamento> Simular(Dinheiro valorLote, Dinheiro entrada, int numeroParcelas, Percentual? desconto = null)
        {
            if (numeroParcelas <= 0)
                return Result<SimulacaoParcelamento>.Failure("Número de parcelas deve ser maior que zero.");

            var valorComDesconto = desconto is not null ? valorLote - (valorLote * desconto) : valorLote;

            if (entrada.Valor > valorComDesconto.Valor)
                return Result<SimulacaoParcelamento>.Failure("Entrada não pode ser maior que o valor do lote (com desconto).");

            var saldoFinanciado = valorComDesconto - entrada;
            var valorParcelaResult = Dinheiro.Criar(Math.Round(saldoFinanciado.Valor / numeroParcelas, 2, MidpointRounding.AwayFromZero));

            return Result<SimulacaoParcelamento>.Success(new SimulacaoParcelamento(valorLote, desconto, valorComDesconto, entrada, saldoFinanciado, numeroParcelas, valorParcelaResult.Value!));
        }
    }
}

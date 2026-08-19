using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed record SimulacaoParcelamento(
     Dinheiro ValorLote, Percentual? Desconto, Dinheiro ValorComDesconto,
     Dinheiro Entrada, Dinheiro SaldoFinanciado, int NumeroParcelas, Dinheiro ValorParcela);
}

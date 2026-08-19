using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.PagamentosAluguel.Queries
{
    public sealed record ListarPagamentosAluguelQuery(Guid LocacaoId) : IQuery<List<PagamentoAluguelItemDto>>;

    public sealed record PagamentoAluguelItemDto(
        Guid Id, int Mes, int Ano, decimal ValorDevido, DateOnly DataVencimento,
        StatusPagamentoAluguel Status, DateOnly? DataPagamento, decimal? ValorPago);
}

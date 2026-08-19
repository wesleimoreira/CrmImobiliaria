using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vendas.Queries
{
    public sealed record ListarVendasQuery(string? Termo) : IQuery<List<VendaListaItemDto>>;

    public sealed record VendaListaItemDto(
        Guid Id, string ClienteNome, string ImovelEndereco, decimal ValorFinal, DateOnly DataVenda, StatusVenda Status);
}

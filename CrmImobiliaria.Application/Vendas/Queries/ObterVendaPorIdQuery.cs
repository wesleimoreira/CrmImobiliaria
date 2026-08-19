using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vendas.Queries
{
    public sealed record ObterVendaPorIdQuery(Guid Id) : IQuery<VendaDetalheDto?>;

    public sealed record VendaDetalheDto(
        Guid Id,
        Guid ClienteId,
        string ClienteNome,
        string? ImovelEndereco,
        Guid CorretorId,
        string CorretorNome,
        decimal ValorFinal,
        DateOnly DataVenda,
        FormaPagamento FormaPagamento,
        string? BancoFinanciamento,
        decimal? ValorFinanciado,
        SituacaoDocumental SituacaoDocumental,
        string? NumeroContrato,
        string? UrlContrato,
        StatusVenda Status,
        string? MotivoDistrato);
}

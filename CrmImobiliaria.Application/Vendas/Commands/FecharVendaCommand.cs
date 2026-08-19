using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed record FecharVendaCommand(
        Guid PropostaId,
        DateOnly DataVenda,
        FormaPagamento FormaPagamento,
        string? BancoFinanciamento,
        decimal? ValorFinanciado) : ICommand<Guid>;
}

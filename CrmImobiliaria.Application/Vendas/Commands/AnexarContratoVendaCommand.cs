using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed record AnexarContratoVendaCommand(Guid Id, string NumeroContrato, string? UrlContrato) : ICommand<bool>;
}

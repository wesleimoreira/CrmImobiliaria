using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed record DistratarVendaCommand(Guid Id, string Motivo) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed record ReabrirLeadCommand(Guid Id) : ICommand<bool>;
}

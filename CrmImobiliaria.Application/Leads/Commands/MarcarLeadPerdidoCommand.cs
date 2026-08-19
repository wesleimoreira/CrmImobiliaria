using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed record MarcarLeadPerdidoCommand(Guid Id, string Motivo) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed record CriarLeadCommand(Guid ClienteId, Guid CorretorId) : ICommand<Guid>;
}

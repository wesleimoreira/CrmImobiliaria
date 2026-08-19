using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed record TransferirCorretorLeadCommand(Guid Id, Guid NovoCorretorId) : ICommand<bool>;
}

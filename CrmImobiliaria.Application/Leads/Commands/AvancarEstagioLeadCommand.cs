using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed record AvancarEstagioLeadCommand(Guid Id, EstagioFunil EstagioAlvo, string? Observacao) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed record AceitarPropostaCommand(Guid Id) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed record CancelarPropostaCommand(Guid Id) : ICommand<bool>;
}

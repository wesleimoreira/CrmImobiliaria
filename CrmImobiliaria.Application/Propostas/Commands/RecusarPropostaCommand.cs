using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed record RecusarPropostaCommand(Guid Id, string Motivo) : ICommand<bool>;
}

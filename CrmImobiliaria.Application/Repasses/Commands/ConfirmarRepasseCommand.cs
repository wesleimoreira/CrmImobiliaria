using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed record ConfirmarRepasseCommand(Guid Id, DateOnly DataRepasse, string Comprovante) : ICommand<bool>;
}

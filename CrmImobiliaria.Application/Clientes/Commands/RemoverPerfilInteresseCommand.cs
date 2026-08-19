using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed record RemoverPerfilInteresseCommand(Guid ClienteId, Guid PerfilId) : ICommand<bool>;
}

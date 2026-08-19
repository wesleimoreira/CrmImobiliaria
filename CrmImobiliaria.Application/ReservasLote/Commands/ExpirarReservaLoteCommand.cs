using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed record ExpirarReservaLoteCommand(Guid Id) : ICommand<bool>;
}

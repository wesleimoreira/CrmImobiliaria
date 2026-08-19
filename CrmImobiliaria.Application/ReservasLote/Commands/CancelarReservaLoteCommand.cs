using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed record CancelarReservaLoteCommand(Guid Id) : ICommand<bool>;
}

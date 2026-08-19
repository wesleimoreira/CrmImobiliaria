using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed record ConverterReservaLoteCommand(Guid Id) : ICommand<bool>;
}

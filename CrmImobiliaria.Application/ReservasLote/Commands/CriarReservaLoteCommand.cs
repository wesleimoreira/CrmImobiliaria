using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed record CriarReservaLoteCommand(Guid LoteId, Guid ClienteId, Guid CorretorId, DateOnly DataReserva, int PrazoDias) : ICommand<Guid>;
}

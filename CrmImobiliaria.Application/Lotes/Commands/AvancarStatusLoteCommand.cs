using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Lotes.Commands
{
    public enum AcaoLote { Reservar, IniciarProposta, Vender, LiberarReserva, Distratar, Bloquear, Desbloquear }

    public sealed record AvancarStatusLoteCommand(Guid Id, AcaoLote Acao, string? Motivo = null) : ICommand<bool>;
}

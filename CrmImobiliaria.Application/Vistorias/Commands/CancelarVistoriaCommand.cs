using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed record CancelarVistoriaCommand(Guid Id, string? Motivo) : ICommand<bool>;
}

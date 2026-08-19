using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public sealed record AgendarVisitaCommand(
        Guid ClienteId, Guid ImovelId, Guid AnuncioImovelId, Guid CorretorId, DateTime DataHora) : ICommand<Guid>;
}

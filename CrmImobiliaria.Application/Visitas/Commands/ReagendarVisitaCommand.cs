using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public sealed record ReagendarVisitaCommand(Guid Id, DateTime NovaDataHora) : ICommand<bool>;
}

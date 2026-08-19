using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Application.Events
{
    // Resolve IDomainEventHandler<T> pelo tipo concreto do evento via reflection — a mesma ideia
    // que o MediatR usa por dentro, só que sem a dependência (que passou a exigir licença paga).
    public sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
    {
        public async Task DispatchAsync(IDomainEvent evento, CancellationToken cancellationToken = default)
        {
            var tipoHandler = typeof(IDomainEventHandler<>).MakeGenericType(evento.GetType());
            var tipoLista = typeof(IEnumerable<>).MakeGenericType(tipoHandler);
            var handlers = (IEnumerable<object>)serviceProvider.GetService(tipoLista)!;
            var metodo = tipoHandler.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))!;

            foreach (var handler in handlers)
                await (Task)metodo.Invoke(handler, [evento, cancellationToken])!;
        }
    }
}

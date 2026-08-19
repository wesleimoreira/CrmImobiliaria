using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Application.Abstractions.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent evento, CancellationToken cancellationToken = default);
    }
}

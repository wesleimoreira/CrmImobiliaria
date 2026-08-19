using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Application.Abstractions.Events
{
    public interface IDomainEventHandler<in TEvento> where TEvento : IDomainEvent
    {
        Task HandleAsync(TEvento evento, CancellationToken cancellationToken = default);
    }
}

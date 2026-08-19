using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<TAggregate?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Adicionar(TAggregate entidade);
        void Remover(TAggregate entidade);
    }
}

using CrmImobiliaria.Application.Abstractions.Persistence;

namespace CrmImobiliaria.Infrastructure.Persistence
{
    public sealed class UnitOfWork(CrmDbContext context) : IUnitOfWork
    {
        public Task<int> SalvarAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
    }
}

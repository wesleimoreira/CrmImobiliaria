using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class LoteRepository(CrmDbContext context) : ILoteRepository
    {
        public Task<Lote?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Lotes.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        public void Adicionar(Lote entidade) => context.Lotes.Add(entidade);
        public void Remover(Lote entidade) => context.Lotes.Remove(entidade);
    }
}

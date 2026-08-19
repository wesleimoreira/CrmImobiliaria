using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class CorretorRepository(CrmDbContext context) : ICorretorRepository
    {
        public Task<Corretor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Corretores.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Adicionar(Corretor entidade) => context.Corretores.Add(entidade);
        public void Remover(Corretor entidade) => context.Corretores.Remove(entidade);
    }
}

using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class PrestadorRepository(CrmDbContext context) : IPrestadorRepository
    {
        public Task<Prestador?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Prestadores.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public void Adicionar(Prestador entidade) => context.Prestadores.Add(entidade);
        public void Remover(Prestador entidade) => context.Prestadores.Remove(entidade);
    }
}

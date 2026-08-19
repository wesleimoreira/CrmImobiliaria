using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class VisitaRepository(CrmDbContext context) : IVisitaRepository
    {
        public Task<Visita?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Visitas.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        public void Adicionar(Visita entidade) => context.Visitas.Add(entidade);
        public void Remover(Visita entidade) => context.Visitas.Remove(entidade);
    }
}

using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class ReservaLoteRepository(CrmDbContext context) : IReservaLoteRepository
    {
        public Task<ReservaLote?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.ReservasLote.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public void Adicionar(ReservaLote entidade) => context.ReservasLote.Add(entidade);
        public void Remover(ReservaLote entidade) => context.ReservasLote.Remove(entidade);
    }
}

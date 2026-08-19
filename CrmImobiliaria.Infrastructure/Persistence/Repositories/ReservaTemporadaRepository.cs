using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class ReservaTemporadaRepository(CrmDbContext context) : IReservaTemporadaRepository
    {
        public Task<ReservaTemporada?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.ReservasTemporada.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public void Adicionar(ReservaTemporada entidade) => context.ReservasTemporada.Add(entidade);
        public void Remover(ReservaTemporada entidade) => context.ReservasTemporada.Remove(entidade);
    }
}

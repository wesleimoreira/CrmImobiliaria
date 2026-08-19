using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class LeadRepository(CrmDbContext context) : ILeadRepository
    {
        public Task<Lead?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Leads.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        public void Adicionar(Lead entidade) => context.Leads.Add(entidade);
        public void Remover(Lead entidade) => context.Leads.Remove(entidade);
    }
}

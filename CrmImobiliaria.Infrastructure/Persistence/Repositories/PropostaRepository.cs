using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class PropostaRepository(CrmDbContext context) : IPropostaRepository
    {
        public Task<Proposta?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Propostas.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public void Adicionar(Proposta entidade) => context.Propostas.Add(entidade);
        public void Remover(Proposta entidade) => context.Propostas.Remove(entidade);
    }
}

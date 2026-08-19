using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class RepasseRepository(CrmDbContext context) : IRepasseRepository
    {
        public Task<Repasse?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Repasses.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public void Adicionar(Repasse entidade) => context.Repasses.Add(entidade);
        public void Remover(Repasse entidade) => context.Repasses.Remove(entidade);

        public Task<bool> ExisteParaPagamentoAsync(Guid pagamentoAluguelId, CancellationToken cancellationToken = default) =>
            context.Repasses.AnyAsync(r => r.PagamentoAluguelId == pagamentoAluguelId, cancellationToken);
    }
}

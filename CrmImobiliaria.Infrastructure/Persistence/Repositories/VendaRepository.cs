using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class VendaRepository(CrmDbContext context) : IVendaRepository
    {
        public Task<Venda?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Vendas.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        public void Adicionar(Venda entidade) => context.Vendas.Add(entidade);
        public void Remover(Venda entidade) => context.Vendas.Remove(entidade);

        public Task<bool> ExisteParaPropostaAsync(Guid propostaId, CancellationToken cancellationToken = default) =>
            context.Vendas.AnyAsync(v => v.PropostaId == propostaId, cancellationToken);
    }
}

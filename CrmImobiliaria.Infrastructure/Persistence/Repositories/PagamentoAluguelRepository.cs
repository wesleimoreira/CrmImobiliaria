using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class PagamentoAluguelRepository(CrmDbContext context) : IPagamentoAluguelRepository
    {
        public Task<PagamentoAluguel?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.PagamentosAluguel.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public void Adicionar(PagamentoAluguel entidade) => context.PagamentosAluguel.Add(entidade);
        public void Remover(PagamentoAluguel entidade) => context.PagamentosAluguel.Remove(entidade);
    }
}

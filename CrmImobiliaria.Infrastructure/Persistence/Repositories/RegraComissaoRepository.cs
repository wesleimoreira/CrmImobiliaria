using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class RegraComissaoRepository(CrmDbContext context) : IRegraComissaoRepository
    {
        public Task<RegraComissao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.RegrasComissao.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        public void Adicionar(RegraComissao entidade) => context.RegrasComissao.Add(entidade);
        public void Remover(RegraComissao entidade) => context.RegrasComissao.Remove(entidade);
    }
}

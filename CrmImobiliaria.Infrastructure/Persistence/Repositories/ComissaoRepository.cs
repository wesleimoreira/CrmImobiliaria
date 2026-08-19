using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class ComissaoRepository(CrmDbContext context) : IComissaoRepository
    {
        public Task<Comissao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Comissoes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Adicionar(Comissao entidade) => context.Comissoes.Add(entidade);
        public void Remover(Comissao entidade) => context.Comissoes.Remove(entidade);

        public Task<bool> ExisteParaOrigemAsync(OrigemComissao origemTipo, Guid origemId, CancellationToken cancellationToken = default) =>
            context.Comissoes.AnyAsync(c => c.OrigemTipo == origemTipo && c.OrigemId == origemId, cancellationToken);
    }
}

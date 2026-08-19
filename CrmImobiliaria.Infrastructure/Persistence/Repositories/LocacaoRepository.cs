using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class LocacaoRepository(CrmDbContext context) : ILocacaoRepository
    {
        public Task<Locacao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Locacoes.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        public void Adicionar(Locacao entidade) => context.Locacoes.Add(entidade);
        public void Remover(Locacao entidade) => context.Locacoes.Remove(entidade);
    }
}

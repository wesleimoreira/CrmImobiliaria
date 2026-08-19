using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class SolicitacaoManutencaoRepository(CrmDbContext context) : ISolicitacaoManutencaoRepository
    {
        public Task<SolicitacaoManutencao?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.SolicitacoesManutencao.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public void Adicionar(SolicitacaoManutencao entidade) => context.SolicitacoesManutencao.Add(entidade);
        public void Remover(SolicitacaoManutencao entidade) => context.SolicitacoesManutencao.Remove(entidade);
    }
}

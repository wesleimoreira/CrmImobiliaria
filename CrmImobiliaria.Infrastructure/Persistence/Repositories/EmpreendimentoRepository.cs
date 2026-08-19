using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class EmpreendimentoRepository(CrmDbContext context) : IEmpreendimentoRepository
    {
        public Task<Empreendimento?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Empreendimentos.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public void Adicionar(Empreendimento entidade) => context.Empreendimentos.Add(entidade);
        public void Remover(Empreendimento entidade) => context.Empreendimentos.Remove(entidade);
    }
}

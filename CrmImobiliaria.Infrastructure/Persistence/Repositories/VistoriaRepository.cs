using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class VistoriaRepository(CrmDbContext context) : IVistoriaRepository
    {
        public Task<Vistoria?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Vistorias.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        public void Adicionar(Vistoria entidade) => context.Vistorias.Add(entidade);
        public void Remover(Vistoria entidade) => context.Vistorias.Remove(entidade);
    }
}

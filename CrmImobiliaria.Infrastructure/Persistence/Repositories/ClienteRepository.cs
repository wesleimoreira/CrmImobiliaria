using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class ClienteRepository(CrmDbContext context) : IClienteRepository
    {
        public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public void Adicionar(Cliente entidade) => context.Clientes.Add(entidade);
        public void Remover(Cliente entidade) => context.Clientes.Remove(entidade);
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Prestadores.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarPrestadoresQueryHandler(CrmDbContext context) : IQueryHandler<ListarPrestadoresQuery, List<PrestadorListaItemDto>>
    {
        public async Task<Result<List<PrestadorListaItemDto>>> HandleAsync(ListarPrestadoresQuery query, CancellationToken cancellationToken = default)
        {
            var prestadores = await context.Prestadores.AsNoTracking().ToListAsync(cancellationToken);

            var itens = prestadores
                .Select(p => new PrestadorListaItemDto(p.Id, p.Nome, p.Telefone.Formatado, p.Email?.Endereco, p.Especialidade, p.Ativo))
                .OrderBy(p => p.Nome)
                .ToList();

            return Result<List<PrestadorListaItemDto>>.Success(itens);
        }
    }
}

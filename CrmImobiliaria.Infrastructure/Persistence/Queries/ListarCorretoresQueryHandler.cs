using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Corretores.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarCorretoresQueryHandler(CrmDbContext context) : IQueryHandler<ListarCorretoresQuery, List<CorretorResumoDto>>
    {
        public async Task<Result<List<CorretorResumoDto>>> HandleAsync(ListarCorretoresQuery query, CancellationToken cancellationToken = default)
        {
            var corretores = await context.Corretores.AsNoTracking().ToListAsync(cancellationToken);

            var itens = corretores
                .Select(c => new CorretorResumoDto(c.Id, c.Nome, c.Creci.ToString()!, c.Telefone.Formatado, c.Email.Endereco, c.Status.ToString()))
                .ToList();

            return Result<List<CorretorResumoDto>>.Success(itens);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Comissoes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarComissoesQueryHandler(CrmDbContext context) : IQueryHandler<ListarComissoesQuery, List<ComissaoListaItemDto>>
    {
        public async Task<Result<List<ComissaoListaItemDto>>> HandleAsync(ListarComissoesQuery query, CancellationToken cancellationToken = default)
        {
            var comissoes = await context.Comissoes.AsNoTracking().ToListAsync(cancellationToken);

            var itens = comissoes
                .Select(c => new ComissaoListaItemDto(
                    c.Id, c.OrigemTipo, c.ValorBase.Valor, c.ComissaoTotal.Valor, c.ComissaoRecebida.Valor, c.ComissaoDistribuida.Valor))
                .ToList();

            return Result<List<ComissaoListaItemDto>>.Success(itens);
        }
    }
}

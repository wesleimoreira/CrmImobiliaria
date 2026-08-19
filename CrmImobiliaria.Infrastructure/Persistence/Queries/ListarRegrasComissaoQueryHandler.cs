using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Comissoes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarRegrasComissaoQueryHandler(CrmDbContext context) : IQueryHandler<ListarRegrasComissaoQuery, List<RegraComissaoListaItemDto>>
    {
        public async Task<Result<List<RegraComissaoListaItemDto>>> HandleAsync(ListarRegrasComissaoQuery query, CancellationToken cancellationToken = default)
        {
            var regras = await context.RegrasComissao.AsNoTracking().ToListAsync(cancellationToken);
            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);

            var itens = regras
                .Select(r => new RegraComissaoListaItemDto(
                    r.Id,
                    r.Nome,
                    r.PercentualComissaoTotal.Valor,
                    r.ImovelId is { } id && imoveis.TryGetValue(id, out var endereco) ? endereco : null,
                    r.Rateio.Count))
                .ToList();

            return Result<List<RegraComissaoListaItemDto>>.Success(itens);
        }
    }
}

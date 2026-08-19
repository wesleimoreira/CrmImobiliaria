using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Empreendimentos.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarEmpreendimentosQueryHandler(CrmDbContext context) : IQueryHandler<ListarEmpreendimentosQuery, List<EmpreendimentoListaItemDto>>
    {
        public async Task<Result<List<EmpreendimentoListaItemDto>>> HandleAsync(ListarEmpreendimentosQuery query, CancellationToken cancellationToken = default)
        {
            var empreendimentos = await context.Empreendimentos.AsNoTracking().ToListAsync(cancellationToken);

            var itens = empreendimentos
                .Select(e => new EmpreendimentoListaItemDto(
                    e.Id, e.Nome, e.LoteadoraIncorporadora, e.Localizacao.ToString(), e.TotalLotes, e.NumeroQuadras, e.Status))
                .OrderBy(e => e.Nome)
                .ToList();

            return Result<List<EmpreendimentoListaItemDto>>.Success(itens);
        }
    }
}

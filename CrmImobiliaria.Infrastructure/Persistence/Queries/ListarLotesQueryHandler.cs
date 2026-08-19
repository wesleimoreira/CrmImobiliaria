using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Lotes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarLotesQueryHandler(CrmDbContext context) : IQueryHandler<ListarLotesQuery, List<LoteListaItemDto>>
    {
        public async Task<Result<List<LoteListaItemDto>>> HandleAsync(ListarLotesQuery query, CancellationToken cancellationToken = default)
        {
            var lotesQuery = context.Lotes.AsNoTracking().AsQueryable();
            if (query.EmpreendimentoId is not null)
                lotesQuery = lotesQuery.Where(l => l.EmpreendimentoId == query.EmpreendimentoId);

            var lotes = await lotesQuery.ToListAsync(cancellationToken);

            var empreendimentos = await context.Empreendimentos.AsNoTracking().ToDictionaryAsync(e => e.Id, e => e.Nome, cancellationToken);

            var itens = lotes
                .Select(l => new LoteListaItemDto(
                    l.Id, l.EmpreendimentoId, empreendimentos.TryGetValue(l.EmpreendimentoId, out var nome) ? nome : "—",
                    l.Quadra, l.Numero, l.Area.MetrosQuadrados, l.ValorVigente.Valor, l.Status))
                .OrderBy(l => l.EmpreendimentoNome).ThenBy(l => l.Quadra).ThenBy(l => l.Numero)
                .ToList();

            return Result<List<LoteListaItemDto>>.Success(itens);
        }
    }
}

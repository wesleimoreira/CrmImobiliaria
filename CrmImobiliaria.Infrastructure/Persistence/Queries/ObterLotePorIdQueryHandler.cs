using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Lotes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterLotePorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterLotePorIdQuery, LoteDetalheDto?>
    {
        public async Task<Result<LoteDetalheDto?>> HandleAsync(ObterLotePorIdQuery query, CancellationToken cancellationToken = default)
        {
            var lote = await context.Lotes.AsNoTracking().FirstOrDefaultAsync(l => l.Id == query.Id, cancellationToken);
            if (lote is null)
                return Result<LoteDetalheDto?>.Success(null);

            var empreendimento = await context.Empreendimentos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == lote.EmpreendimentoId, cancellationToken);

            var dto = new LoteDetalheDto(
                lote.Id, lote.EmpreendimentoId, empreendimento?.Nome ?? "—", lote.Quadra, lote.Numero, lote.Area.MetrosQuadrados,
                lote.Valor.Valor, lote.EntradaMinima.Valor, lote.ParcelamentoMaximo, lote.ValorPromocional?.Valor, lote.ValorVigente.Valor, lote.Status);

            return Result<LoteDetalheDto?>.Success(dto);
        }
    }
}

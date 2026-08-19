using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.ReservasLote.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterReservaLotePorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterReservaLotePorIdQuery, ReservaLoteDetalheDto?>
    {
        public async Task<Result<ReservaLoteDetalheDto?>> HandleAsync(ObterReservaLotePorIdQuery query, CancellationToken cancellationToken = default)
        {
            var reserva = await context.ReservasLote.AsNoTracking().FirstOrDefaultAsync(r => r.Id == query.Id, cancellationToken);
            if (reserva is null)
                return Result<ReservaLoteDetalheDto?>.Success(null);

            var lote = await context.Lotes.AsNoTracking().FirstOrDefaultAsync(l => l.Id == reserva.LoteId, cancellationToken);
            var loteDescricao = lote is not null ? $"Quadra {lote.Quadra}, Lote {lote.Numero}" : "—";
            var clienteNome = (await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == reserva.ClienteId, cancellationToken))?.Nome ?? "—";
            var corretorNome = (await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == reserva.CorretorId, cancellationToken))?.Nome ?? "—";

            var dto = new ReservaLoteDetalheDto(
                reserva.Id, reserva.LoteId, loteDescricao, clienteNome, corretorNome,
                reserva.DataReserva, reserva.DataExpiracao, reserva.Status);

            return Result<ReservaLoteDetalheDto?>.Success(dto);
        }
    }
}

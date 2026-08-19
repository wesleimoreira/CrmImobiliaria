using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.ReservasLote.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarReservasLoteQueryHandler(CrmDbContext context) : IQueryHandler<ListarReservasLoteQuery, List<ReservaLoteListaItemDto>>
    {
        public async Task<Result<List<ReservaLoteListaItemDto>>> HandleAsync(ListarReservasLoteQuery query, CancellationToken cancellationToken = default)
        {
            var reservas = await context.ReservasLote.AsNoTracking().ToListAsync(cancellationToken);

            var lotes = await context.Lotes.AsNoTracking().ToDictionaryAsync(l => l.Id, l => l, cancellationToken);
            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);
            var corretores = await context.Corretores.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);

            var itens = reservas
                .Select(r =>
                {
                    var loteDescricao = lotes.TryGetValue(r.LoteId, out var lote) ? $"Quadra {lote.Quadra}, Lote {lote.Numero}" : "—";
                    var clienteNome = clientes.TryGetValue(r.ClienteId, out var cliente) ? cliente : "—";
                    var corretorNome = corretores.TryGetValue(r.CorretorId, out var corretor) ? corretor : "—";

                    return new ReservaLoteListaItemDto(
                        r.Id, loteDescricao, clienteNome, corretorNome, r.DataReserva, r.DataExpiracao, r.Status);
                })
                .OrderByDescending(r => r.DataReserva)
                .ToList();

            return Result<List<ReservaLoteListaItemDto>>.Success(itens);
        }
    }
}

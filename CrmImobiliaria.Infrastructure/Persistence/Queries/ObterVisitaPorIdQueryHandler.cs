using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Visitas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterVisitaPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterVisitaPorIdQuery, VisitaDetalheDto?>
    {
        public async Task<Result<VisitaDetalheDto?>> HandleAsync(ObterVisitaPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var visita = await context.Visitas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
            if (visita is null)
                return Result<VisitaDetalheDto?>.Success(null);

            var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == visita.ClienteId, cancellationToken);
            var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == visita.ImovelId, cancellationToken);
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == visita.CorretorId, cancellationToken);

            var dto = new VisitaDetalheDto(
                visita.Id, cliente?.Nome ?? "—", imovel?.Endereco.ToString() ?? "—", corretor?.Nome ?? "—",
                visita.DataHora, visita.Status, visita.Feedback);

            return Result<VisitaDetalheDto?>.Success(dto);
        }
    }
}

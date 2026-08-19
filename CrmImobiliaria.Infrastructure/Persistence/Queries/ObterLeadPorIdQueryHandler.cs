using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Leads.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterLeadPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterLeadPorIdQuery, LeadDetalheDto?>
    {
        public async Task<Result<LeadDetalheDto?>> HandleAsync(ObterLeadPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var lead = await context.Leads.AsNoTracking().FirstOrDefaultAsync(l => l.Id == query.Id, cancellationToken);
            if (lead is null)
                return Result<LeadDetalheDto?>.Success(null);

            var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == lead.ClienteId, cancellationToken);
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == lead.CorretorId, cancellationToken);

            var historico = lead.Historico
                .Select(h => new HistoricoItemDto(h.Estagio, h.DataHora, h.Observacao))
                .ToList();

            var dto = new LeadDetalheDto(
                lead.Id, lead.ClienteId, cliente?.Nome ?? "—", lead.CorretorId, corretor?.Nome ?? "—",
                lead.EstagioAtual, lead.MotivoPerda, lead.CriadoEm, historico);

            return Result<LeadDetalheDto?>.Success(dto);
        }
    }
}

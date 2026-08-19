using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Leads.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarLeadsQueryHandler(CrmDbContext context) : IQueryHandler<ListarLeadsQuery, List<LeadListaItemDto>>
    {
        public async Task<Result<List<LeadListaItemDto>>> HandleAsync(ListarLeadsQuery query, CancellationToken cancellationToken = default)
        {
            var consulta = context.Leads.AsNoTracking()
                .Join(context.Clientes.AsNoTracking(), l => l.ClienteId, c => c.Id, (l, c) => new { Lead = l, ClienteNome = c.Nome })
                .Join(context.Corretores.AsNoTracking(), x => x.Lead.CorretorId, cr => cr.Id, (x, cr) => new { x.Lead, x.ClienteNome, CorretorNome = cr.Nome });

            if (!string.IsNullOrWhiteSpace(query.Termo))
                consulta = consulta.Where(x => x.ClienteNome.Contains(query.Termo));

            var itens = await consulta
                .Select(x => new LeadListaItemDto(x.Lead.Id, x.ClienteNome, x.CorretorNome, x.Lead.EstagioAtual, x.Lead.CriadoEm))
                .ToListAsync(cancellationToken);

            return Result<List<LeadListaItemDto>>.Success(itens);
        }
    }
}

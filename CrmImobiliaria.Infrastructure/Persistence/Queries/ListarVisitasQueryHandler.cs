using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Visitas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarVisitasQueryHandler(CrmDbContext context) : IQueryHandler<ListarVisitasQuery, List<VisitaListaItemDto>>
    {
        public async Task<Result<List<VisitaListaItemDto>>> HandleAsync(ListarVisitasQuery query, CancellationToken cancellationToken = default)
        {
            var consulta = context.Visitas.AsNoTracking()
                .Join(context.Clientes.AsNoTracking(), v => v.ClienteId, c => c.Id, (v, c) => new { Visita = v, ClienteNome = c.Nome })
                .Join(context.Imoveis.AsNoTracking(), x => x.Visita.ImovelId, i => i.Id, (x, i) => new { x.Visita, x.ClienteNome, Imovel = i })
                .Join(context.Corretores.AsNoTracking(), x => x.Visita.CorretorId, cr => cr.Id, (x, cr) => new { x.Visita, x.ClienteNome, x.Imovel, CorretorNome = cr.Nome });

            if (!string.IsNullOrWhiteSpace(query.Termo))
                consulta = consulta.Where(x => x.ClienteNome.Contains(query.Termo));

            var brutos = await consulta
                .Select(x => new { x.Visita.Id, x.ClienteNome, x.Imovel.Endereco, x.CorretorNome, x.Visita.DataHora, x.Visita.Status })
                .ToListAsync(cancellationToken);

            var itens = brutos
                .Select(x => new VisitaListaItemDto(x.Id, x.ClienteNome, x.Endereco.ToString(), x.CorretorNome, x.DataHora, x.Status))
                .ToList();

            return Result<List<VisitaListaItemDto>>.Success(itens);
        }
    }
}

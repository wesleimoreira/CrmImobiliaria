using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Vistorias.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarVistoriasQueryHandler(CrmDbContext context) : IQueryHandler<ListarVistoriasQuery, List<VistoriaListaItemDto>>
    {
        public async Task<Result<List<VistoriaListaItemDto>>> HandleAsync(ListarVistoriasQuery query, CancellationToken cancellationToken = default)
        {
            var vistorias = await context.Vistorias.AsNoTracking().ToListAsync(cancellationToken);

            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);
            var corretores = await context.Corretores.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);

            var itens = vistorias
                .Select(v => new VistoriaListaItemDto(
                    v.Id,
                    imoveis.TryGetValue(v.ImovelId, out var endereco) ? endereco : "—",
                    v.Tipo, v.DataAgendada, v.DataRealizada,
                    corretores.TryGetValue(v.ResponsavelId, out var nome) ? nome : "—",
                    v.Status))
                .OrderByDescending(v => v.DataAgendada)
                .ToList();

            return Result<List<VistoriaListaItemDto>>.Success(itens);
        }
    }
}

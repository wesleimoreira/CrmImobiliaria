using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Propostas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarPropostasQueryHandler(CrmDbContext context) : IQueryHandler<ListarPropostasQuery, List<PropostaListaItemDto>>
    {
        public async Task<Result<List<PropostaListaItemDto>>> HandleAsync(ListarPropostasQuery query, CancellationToken cancellationToken = default)
        {
            var propostas = await context.Propostas.AsNoTracking().ToListAsync(cancellationToken);

            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);
            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);

            var itens = propostas
                .Select(p => new PropostaListaItemDto(
                    p.Id,
                    clientes.TryGetValue(p.ClienteId, out var clienteNome) ? clienteNome : "—",
                    p.ImovelId is { } id && imoveis.TryGetValue(id, out var endereco) ? endereco : "—",
                    p.UltimoValorNegociado.Valor,
                    p.Status,
                    p.CriadoEm))
                .Where(item => string.IsNullOrWhiteSpace(query.Termo) || item.ClienteNome.Contains(query.Termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Result<List<PropostaListaItemDto>>.Success(itens);
        }
    }
}

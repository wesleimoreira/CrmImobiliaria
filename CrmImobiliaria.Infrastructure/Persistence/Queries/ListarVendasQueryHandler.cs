using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Vendas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarVendasQueryHandler(CrmDbContext context) : IQueryHandler<ListarVendasQuery, List<VendaListaItemDto>>
    {
        public async Task<Result<List<VendaListaItemDto>>> HandleAsync(ListarVendasQuery query, CancellationToken cancellationToken = default)
        {
            var vendas = await context.Vendas.AsNoTracking().ToListAsync(cancellationToken);

            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);
            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);

            var itens = vendas
                .Select(v => new VendaListaItemDto(
                    v.Id,
                    clientes.TryGetValue(v.ClienteId, out var clienteNome) ? clienteNome : "—",
                    v.ImovelId is { } id && imoveis.TryGetValue(id, out var endereco) ? endereco : "—",
                    v.ValorFinal.Valor,
                    v.DataVenda,
                    v.Status))
                .Where(item => string.IsNullOrWhiteSpace(query.Termo) || item.ClienteNome.Contains(query.Termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Result<List<VendaListaItemDto>>.Success(itens);
        }
    }
}

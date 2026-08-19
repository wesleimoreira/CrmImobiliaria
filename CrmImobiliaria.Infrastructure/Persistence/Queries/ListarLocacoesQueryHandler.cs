using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Locacoes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarLocacoesQueryHandler(CrmDbContext context) : IQueryHandler<ListarLocacoesQuery, List<LocacaoListaItemDto>>
    {
        public async Task<Result<List<LocacaoListaItemDto>>> HandleAsync(ListarLocacoesQuery query, CancellationToken cancellationToken = default)
        {
            var locacoes = await context.Locacoes.AsNoTracking().ToListAsync(cancellationToken);

            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);
            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);

            var itens = locacoes
                .Select(l => new LocacaoListaItemDto(
                    l.Id,
                    l.ImovelId,
                    clientes.TryGetValue(l.LocatarioId, out var locatarioNome) ? locatarioNome : "—",
                    imoveis.TryGetValue(l.ImovelId, out var endereco) ? endereco : "—",
                    l.EstagioAtual,
                    l.Status,
                    l.ValorAluguel?.Valor))
                .Where(item => string.IsNullOrWhiteSpace(query.Termo) || item.LocatarioNome.Contains(query.Termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Result<List<LocacaoListaItemDto>>.Success(itens);
        }
    }
}

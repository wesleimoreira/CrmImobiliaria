using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Repasses.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarRepassesQueryHandler(CrmDbContext context) : IQueryHandler<ListarRepassesQuery, List<RepasseListaItemDto>>
    {
        public async Task<Result<List<RepasseListaItemDto>>> HandleAsync(ListarRepassesQuery query, CancellationToken cancellationToken = default)
        {
            var repassesQuery = context.Repasses.AsNoTracking().AsQueryable();
            if (query.LocacaoId is not null)
                repassesQuery = repassesQuery.Where(r => r.LocacaoId == query.LocacaoId);

            var repasses = await repassesQuery.ToListAsync(cancellationToken);

            var locacoes = await context.Locacoes.AsNoTracking().ToDictionaryAsync(l => l.Id, l => l, cancellationToken);
            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);
            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);

            var itens = repasses
                .Select(r =>
                {
                    locacoes.TryGetValue(r.LocacaoId, out var locacao);
                    var imovel = locacao is not null && imoveis.TryGetValue(locacao.ImovelId, out var endereco) ? endereco : "—";
                    var proprietario = locacao is not null && clientes.TryGetValue(locacao.ProprietarioId, out var nome) ? nome : "—";

                    return new RepasseListaItemDto(
                        r.Id, r.LocacaoId, imovel, proprietario, r.Competencia.Mes, r.Competencia.Ano,
                        r.ValorAluguelRecebido.Valor, r.ValorLiquido.Valor, r.Status);
                })
                .OrderByDescending(r => r.Ano).ThenByDescending(r => r.Mes)
                .ToList();

            return Result<List<RepasseListaItemDto>>.Success(itens);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.SolicitacoesManutencao.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarSolicitacoesManutencaoQueryHandler(CrmDbContext context)
        : IQueryHandler<ListarSolicitacoesManutencaoQuery, List<SolicitacaoManutencaoListaItemDto>>
    {
        public async Task<Result<List<SolicitacaoManutencaoListaItemDto>>> HandleAsync(ListarSolicitacoesManutencaoQuery query, CancellationToken cancellationToken = default)
        {
            var solicitacoes = await context.SolicitacoesManutencao.AsNoTracking().ToListAsync(cancellationToken);

            var imoveis = await context.Imoveis.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i.Endereco.ToString(), cancellationToken);
            var clientes = await context.Clientes.AsNoTracking().ToDictionaryAsync(c => c.Id, c => c.Nome, cancellationToken);

            var itens = solicitacoes
                .Select(s => new SolicitacaoManutencaoListaItemDto(
                    s.Id,
                    imoveis.TryGetValue(s.ImovelId, out var endereco) ? endereco : "—",
                    clientes.TryGetValue(s.SolicitanteId, out var nome) ? nome : "—",
                    s.Descricao, s.Status))
                .ToList();

            return Result<List<SolicitacaoManutencaoListaItemDto>>.Success(itens);
        }
    }
}

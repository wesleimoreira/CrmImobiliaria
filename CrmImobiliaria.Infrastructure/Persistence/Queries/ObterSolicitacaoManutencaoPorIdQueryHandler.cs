using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.SolicitacoesManutencao.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterSolicitacaoManutencaoPorIdQueryHandler(CrmDbContext context)
        : IQueryHandler<ObterSolicitacaoManutencaoPorIdQuery, SolicitacaoManutencaoDetalheDto?>
    {
        public async Task<Result<SolicitacaoManutencaoDetalheDto?>> HandleAsync(ObterSolicitacaoManutencaoPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var solicitacao = await context.SolicitacoesManutencao.AsNoTracking().FirstOrDefaultAsync(s => s.Id == query.Id, cancellationToken);
            if (solicitacao is null)
                return Result<SolicitacaoManutencaoDetalheDto?>.Success(null);

            var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == solicitacao.ImovelId, cancellationToken);
            var solicitante = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == solicitacao.SolicitanteId, cancellationToken);
            var prestadores = await context.Prestadores.AsNoTracking().ToDictionaryAsync(p => p.Id, p => p.Nome, cancellationToken);

            var dto = new SolicitacaoManutencaoDetalheDto(
                solicitacao.Id, imovel?.Endereco.ToString() ?? "—", solicitante?.Nome ?? "—", solicitacao.Descricao, solicitacao.Status,
                solicitacao.OrcamentoAprovadoId, solicitacao.DataConclusao,
                solicitacao.Orcamentos.Select(o => new OrcamentoItemDto(
                    o.Id, prestadores.TryGetValue(o.PrestadorId, out var nome) ? nome : "—", o.Valor.Valor, o.Descricao, o.Status)).ToList());

            return Result<SolicitacaoManutencaoDetalheDto?>.Success(dto);
        }
    }
}

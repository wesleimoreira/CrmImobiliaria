using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.PagamentosAluguel.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarPagamentosAluguelQueryHandler(CrmDbContext context)
        : IQueryHandler<ListarPagamentosAluguelQuery, List<PagamentoAluguelItemDto>>
    {
        public async Task<Result<List<PagamentoAluguelItemDto>>> HandleAsync(ListarPagamentosAluguelQuery query, CancellationToken cancellationToken = default)
        {
            var pagamentos = await context.PagamentosAluguel.AsNoTracking()
                .Where(p => p.LocacaoId == query.LocacaoId)
                .ToListAsync(cancellationToken);

            var itens = pagamentos
                .OrderBy(p => p.Competencia.Ano).ThenBy(p => p.Competencia.Mes)
                .Select(p => new PagamentoAluguelItemDto(
                    p.Id, p.Competencia.Mes, p.Competencia.Ano, p.ValorDevido.Valor, p.DataVencimento,
                    p.Status, p.DataPagamento, p.ValorPago?.Valor))
                .ToList();

            return Result<List<PagamentoAluguelItemDto>>.Success(itens);
        }
    }
}

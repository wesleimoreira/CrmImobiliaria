using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Repasses.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterRepassePorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterRepassePorIdQuery, RepasseDetalheDto?>
    {
        public async Task<Result<RepasseDetalheDto?>> HandleAsync(ObterRepassePorIdQuery query, CancellationToken cancellationToken = default)
        {
            var repasse = await context.Repasses.AsNoTracking().FirstOrDefaultAsync(r => r.Id == query.Id, cancellationToken);
            if (repasse is null)
                return Result<RepasseDetalheDto?>.Success(null);

            var locacao = await context.Locacoes.AsNoTracking().FirstOrDefaultAsync(l => l.Id == repasse.LocacaoId, cancellationToken);
            var imovel = locacao is not null
                ? (await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == locacao.ImovelId, cancellationToken))?.Endereco.ToString() ?? "—"
                : "—";
            var proprietario = locacao is not null
                ? (await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == locacao.ProprietarioId, cancellationToken))?.Nome ?? "—"
                : "—";

            var dto = new RepasseDetalheDto(
                repasse.Id, repasse.LocacaoId, imovel, proprietario, repasse.Competencia.Mes, repasse.Competencia.Ano,
                repasse.ValorAluguelRecebido.Valor, repasse.TaxaAdministracao.Valor, repasse.ValorTaxaAdministracao.Valor,
                repasse.Despesas.Select(d => new DespesaRepasseDto(d.Descricao, d.Valor.Valor)).ToList(),
                repasse.ValorLiquido.Valor, repasse.Status, repasse.DataRepasse, repasse.Comprovante);

            return Result<RepasseDetalheDto?>.Success(dto);
        }
    }
}

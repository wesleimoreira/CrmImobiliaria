using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Locacoes.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterLocacaoPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterLocacaoPorIdQuery, LocacaoDetalheDto?>
    {
        public async Task<Result<LocacaoDetalheDto?>> HandleAsync(ObterLocacaoPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var locacao = await context.Locacoes.AsNoTracking().FirstOrDefaultAsync(l => l.Id == query.Id, cancellationToken);
            if (locacao is null)
                return Result<LocacaoDetalheDto?>.Success(null);

            var proprietario = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == locacao.ProprietarioId, cancellationToken);
            var locatario = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == locacao.LocatarioId, cancellationToken);
            var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == locacao.ImovelId, cancellationToken);
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == locacao.CorretorId, cancellationToken);

            var dto = new LocacaoDetalheDto(
                locacao.Id, proprietario?.Nome ?? "—", locatario?.Nome ?? "—", imovel?.Endereco.ToString() ?? "—", corretor?.Nome ?? "—",
                locacao.EstagioAtual, locacao.Status, locacao.ValorAluguel?.Valor,
                locacao.Periodo?.DataInicial, locacao.Periodo?.DataFinal, locacao.DiaVencimento,
                locacao.Garantia?.Valor, locacao.TaxaAdministracao?.Valor, locacao.IndiceReajuste,
                locacao.MotivoReprovacaoOuCancelamento);

            return Result<LocacaoDetalheDto?>.Success(dto);
        }
    }
}

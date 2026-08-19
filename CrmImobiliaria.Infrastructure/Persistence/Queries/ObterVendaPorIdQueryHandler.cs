using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Vendas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterVendaPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterVendaPorIdQuery, VendaDetalheDto?>
    {
        public async Task<Result<VendaDetalheDto?>> HandleAsync(ObterVendaPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var venda = await context.Vendas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == query.Id, cancellationToken);
            if (venda is null)
                return Result<VendaDetalheDto?>.Success(null);

            var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == venda.ClienteId, cancellationToken);
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == venda.CorretorId, cancellationToken);

            string? imovelEndereco = null;
            if (venda.ImovelId is { } imovelId)
            {
                var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == imovelId, cancellationToken);
                imovelEndereco = imovel?.Endereco.ToString();
            }

            var dto = new VendaDetalheDto(
                venda.Id, venda.ClienteId, cliente?.Nome ?? "—", imovelEndereco, venda.CorretorId, corretor?.Nome ?? "—",
                venda.ValorFinal.Valor, venda.DataVenda, venda.FormaPagamento,
                venda.Financiamento?.Banco, venda.Financiamento?.ValorFinanciado.Valor,
                venda.SituacaoDocumental, venda.NumeroContrato, venda.UrlContrato, venda.Status, venda.MotivoDistrato);

            return Result<VendaDetalheDto?>.Success(dto);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Comissoes.Queries;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterComissaoPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterComissaoPorIdQuery, ComissaoDetalheDto?>
    {
        public async Task<Result<ComissaoDetalheDto?>> HandleAsync(ObterComissaoPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var comissao = await context.Comissoes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);
            if (comissao is null)
                return Result<ComissaoDetalheDto?>.Success(null);

            string? clienteNome = null;
            string? imovelEndereco = null;

            if (comissao.OrigemTipo == OrigemComissao.Venda)
            {
                var venda = await context.Vendas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == comissao.OrigemId, cancellationToken);
                if (venda is not null)
                {
                    var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == venda.ClienteId, cancellationToken);
                    clienteNome = cliente?.Nome;

                    if (venda.ImovelId is { } imovelId)
                    {
                        var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == imovelId, cancellationToken);
                        imovelEndereco = imovel?.Endereco.ToString();
                    }
                }
            }

            var rateio = comissao.Rateio
                .Select(r => new RateioComissaoItemDto(r.Papel, r.Percentual.Valor, r.Valor.Valor, r.BeneficiarioId))
                .ToList();

            var dto = new ComissaoDetalheDto(
                comissao.Id, comissao.OrigemTipo, comissao.OrigemId, clienteNome, imovelEndereco,
                comissao.ValorBase.Valor, comissao.PercentualComissao.Valor, comissao.ComissaoTotal.Valor,
                comissao.ComissaoRecebida.Valor, comissao.ComissaoDistribuida.Valor, comissao.Saldo.Valor, rateio);

            return Result<ComissaoDetalheDto?>.Success(dto);
        }
    }
}

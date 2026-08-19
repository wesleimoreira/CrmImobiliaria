using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Propostas.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterPropostaPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterPropostaPorIdQuery, PropostaDetalheDto?>
    {
        public async Task<Result<PropostaDetalheDto?>> HandleAsync(ObterPropostaPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var proposta = await context.Propostas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
            if (proposta is null)
                return Result<PropostaDetalheDto?>.Success(null);

            var cliente = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == proposta.ClienteId, cancellationToken);
            var corretor = await context.Corretores.AsNoTracking().FirstOrDefaultAsync(c => c.Id == proposta.CorretorId, cancellationToken);

            string? imovelEndereco = null;
            if (proposta.ImovelId is { } imovelId)
            {
                var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == imovelId, cancellationToken);
                imovelEndereco = imovel?.Endereco.ToString();
            }

            var historico = proposta.Historico
                .Select(h => new HistoricoNegociacaoDto(h.Valor.Valor, h.Origem, h.DataHora, h.Observacao))
                .ToList();

            var dto = new PropostaDetalheDto(
                proposta.Id, proposta.ClienteId, cliente?.Nome ?? "—", proposta.ImovelId, proposta.AnuncioImovelId, imovelEndereco,
                proposta.CorretorId, corretor?.Nome ?? "—", proposta.ValorAnunciado.Valor, proposta.UltimoValorNegociado.Valor,
                proposta.Entrada?.Valor, proposta.NumeroParcelas, proposta.ValorParcela?.Valor, proposta.FormaPagamento,
                proposta.Status, proposta.MotivoRecusa, historico);

            return Result<PropostaDetalheDto?>.Success(dto);
        }
    }
}

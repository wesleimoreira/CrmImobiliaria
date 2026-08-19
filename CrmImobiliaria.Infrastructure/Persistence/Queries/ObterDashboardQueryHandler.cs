using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Dashboard.Queries;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterDashboardQueryHandler(CrmDbContext context) : IQueryHandler<ObterDashboardQuery, DashboardDto>
    {
        public async Task<Result<DashboardDto>> HandleAsync(ObterDashboardQuery query, CancellationToken cancellationToken = default)
        {
            var totalClientes = await context.Clientes.AsNoTracking().CountAsync(cancellationToken);

            var imoveis = await context.Imoveis.AsNoTracking().ToListAsync(cancellationToken);
            var imoveisDisponiveis = imoveis.Count(i => i.Anuncios.Any(a => a.Status == StatusAnuncio.Disponivel));

            var leadsEmAndamento = await context.Leads.AsNoTracking()
                .CountAsync(l => l.EstagioAtual != EstagioFunil.Fechado && l.EstagioAtual != EstagioFunil.Perdido, cancellationToken);

            var propostasEmAnalise = await context.Propostas.AsNoTracking()
                .CountAsync(p => p.Status == StatusProposta.EmAnalise || p.Status == StatusProposta.ContraProposta, cancellationToken);

            var hoje = DateOnly.FromDateTime(DateTime.Today);
            var inicioMes = new DateOnly(hoje.Year, hoje.Month, 1);
            var vendas = await context.Vendas.AsNoTracking()
                .Where(v => v.Status == StatusVenda.Concluida && v.DataVenda >= inicioMes)
                .ToListAsync(cancellationToken);
            var vendasNoMes = vendas.Count;
            var valorVendasNoMes = vendas.Sum(v => v.ValorFinal.Valor);

            var locacoesAtivas = await context.Locacoes.AsNoTracking().CountAsync(l => l.Status == StatusLocacao.Ativa, cancellationToken);

            var comissoes = await context.Comissoes.AsNoTracking().ToListAsync(cancellationToken);
            var comissoesAReceber = comissoes.Sum(c => (c.ComissaoTotal - c.ComissaoRecebida).Valor);

            var repassesPendentes = await context.Repasses.AsNoTracking().CountAsync(r => r.Status == StatusRepasse.Pendente, cancellationToken);

            var lotesDisponiveis = await context.Lotes.AsNoTracking().CountAsync(l => l.Status == StatusLote.Disponivel, cancellationToken);
            var lotesVendidos = await context.Lotes.AsNoTracking().CountAsync(l => l.Status == StatusLote.Vendido, cancellationToken);

            var vistoriasAgendadas = await context.Vistorias.AsNoTracking().CountAsync(v => v.Status == StatusVistoria.Agendada, cancellationToken);

            var solicitacoesAbertas = await context.SolicitacoesManutencao.AsNoTracking()
                .CountAsync(s => s.Status != StatusSolicitacaoManutencao.Concluida && s.Status != StatusSolicitacaoManutencao.Cancelada, cancellationToken);

            var reservasAtivas = await context.ReservasLote.AsNoTracking().CountAsync(r => r.Status == StatusReservaLote.Ativa, cancellationToken);

            var dto = new DashboardDto(
                totalClientes, imoveisDisponiveis, leadsEmAndamento, propostasEmAnalise,
                vendasNoMes, valorVendasNoMes, locacoesAtivas, comissoesAReceber, repassesPendentes,
                lotesDisponiveis, lotesVendidos, vistoriasAgendadas, solicitacoesAbertas, reservasAtivas);

            return Result<DashboardDto>.Success(dto);
        }
    }
}

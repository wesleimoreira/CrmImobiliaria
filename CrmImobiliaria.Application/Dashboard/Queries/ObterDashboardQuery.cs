using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Dashboard.Queries
{
    public sealed record ObterDashboardQuery : IQuery<DashboardDto>;

    public sealed record DashboardDto(
        int TotalClientes,
        int ImoveisDisponiveis,
        int LeadsEmAndamento,
        int PropostasEmAnalise,
        int VendasNoMes,
        decimal ValorVendasNoMes,
        int LocacoesAtivas,
        decimal ComissoesAReceber,
        int RepassesPendentes,
        int LotesDisponiveis,
        int LotesVendidos,
        int VistoriasAgendadas,
        int SolicitacoesManutencaoAbertas,
        int ReservasLoteAtivas);
}

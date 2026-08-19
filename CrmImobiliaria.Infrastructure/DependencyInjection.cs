using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Application.Clientes.Queries;
using CrmImobiliaria.Application.Comissoes.Queries;
using CrmImobiliaria.Application.Empreendimentos.Queries;
using CrmImobiliaria.Application.Locacoes.Queries;
using CrmImobiliaria.Application.Lotes.Queries;
using CrmImobiliaria.Application.PagamentosAluguel.Queries;
using CrmImobiliaria.Application.Corretores.Queries;
using CrmImobiliaria.Application.Imoveis.Queries;
using CrmImobiliaria.Application.Leads.Queries;
using CrmImobiliaria.Application.Propostas.Queries;
using CrmImobiliaria.Application.Repasses.Queries;
using CrmImobiliaria.Application.Prestadores.Queries;
using CrmImobiliaria.Application.ReservasLote.Queries;
using CrmImobiliaria.Application.Dashboard.Queries;
using CrmImobiliaria.Application.SolicitacoesManutencao.Queries;
using CrmImobiliaria.Application.Vendas.Queries;
using CrmImobiliaria.Application.Visitas.Queries;
using CrmImobiliaria.Application.Vistorias.Queries;
using CrmImobiliaria.Infrastructure.Persistence;
using CrmImobiliaria.Infrastructure.Persistence.Queries;
using CrmImobiliaria.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrmImobiliaria.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CrmDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CrmImobiliaria")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ICorretorRepository, CorretorRepository>();
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<IImovelRepository, ImovelRepository>();
            services.AddScoped<IVisitaRepository, VisitaRepository>();
            services.AddScoped<IPropostaRepository, PropostaRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<IRegraComissaoRepository, RegraComissaoRepository>();
            services.AddScoped<IComissaoRepository, ComissaoRepository>();
            services.AddScoped<ILocacaoRepository, LocacaoRepository>();
            services.AddScoped<IPagamentoAluguelRepository, PagamentoAluguelRepository>();
            services.AddScoped<IRepasseRepository, RepasseRepository>();
            services.AddScoped<IVistoriaRepository, VistoriaRepository>();
            services.AddScoped<ISolicitacaoManutencaoRepository, SolicitacaoManutencaoRepository>();
            services.AddScoped<IPrestadorRepository, PrestadorRepository>();
            services.AddScoped<IEmpreendimentoRepository, EmpreendimentoRepository>();
            services.AddScoped<ILoteRepository, LoteRepository>();
            services.AddScoped<IReservaLoteRepository, ReservaLoteRepository>();
            services.AddScoped<IReservaTemporadaRepository, ReservaTemporadaRepository>();

            services.AddScoped<IQueryHandler<ListarClientesQuery, List<ClienteListaItemDto>>, ListarClientesQueryHandler>();
            services.AddScoped<IQueryHandler<ObterClientePorIdQuery, ClienteDetalheDto?>, ObterClientePorIdQueryHandler>();
            services.AddScoped<IQueryHandler<ListarCorretoresQuery, List<CorretorResumoDto>>, ListarCorretoresQueryHandler>();
            services.AddScoped<IQueryHandler<ObterCorretorPorIdQuery, CorretorDetalheDto?>, ObterCorretorPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarImoveisQuery, List<ImovelListaItemDto>>, ListarImoveisQueryHandler>();
            services.AddScoped<IQueryHandler<ObterImovelPorIdQuery, ImovelDetalheDto?>, ObterImovelPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarLeadsQuery, List<LeadListaItemDto>>, ListarLeadsQueryHandler>();
            services.AddScoped<IQueryHandler<ObterLeadPorIdQuery, LeadDetalheDto?>, ObterLeadPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarVisitasQuery, List<VisitaListaItemDto>>, ListarVisitasQueryHandler>();
            services.AddScoped<IQueryHandler<ObterVisitaPorIdQuery, VisitaDetalheDto?>, ObterVisitaPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarPropostasQuery, List<PropostaListaItemDto>>, ListarPropostasQueryHandler>();
            services.AddScoped<IQueryHandler<ObterPropostaPorIdQuery, PropostaDetalheDto?>, ObterPropostaPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarVendasQuery, List<VendaListaItemDto>>, ListarVendasQueryHandler>();
            services.AddScoped<IQueryHandler<ObterVendaPorIdQuery, VendaDetalheDto?>, ObterVendaPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarRegrasComissaoQuery, List<RegraComissaoListaItemDto>>, ListarRegrasComissaoQueryHandler>();
            services.AddScoped<IQueryHandler<ListarComissoesQuery, List<ComissaoListaItemDto>>, ListarComissoesQueryHandler>();
            services.AddScoped<IQueryHandler<ObterComissaoPorIdQuery, ComissaoDetalheDto?>, ObterComissaoPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarLocacoesQuery, List<LocacaoListaItemDto>>, ListarLocacoesQueryHandler>();
            services.AddScoped<IQueryHandler<ObterLocacaoPorIdQuery, LocacaoDetalheDto?>, ObterLocacaoPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarPagamentosAluguelQuery, List<PagamentoAluguelItemDto>>, ListarPagamentosAluguelQueryHandler>();

            services.AddScoped<IQueryHandler<ListarRepassesQuery, List<RepasseListaItemDto>>, ListarRepassesQueryHandler>();
            services.AddScoped<IQueryHandler<ObterRepassePorIdQuery, RepasseDetalheDto?>, ObterRepassePorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarPrestadoresQuery, List<PrestadorListaItemDto>>, ListarPrestadoresQueryHandler>();

            services.AddScoped<IQueryHandler<ListarVistoriasQuery, List<VistoriaListaItemDto>>, ListarVistoriasQueryHandler>();
            services.AddScoped<IQueryHandler<ObterVistoriaPorIdQuery, VistoriaDetalheDto?>, ObterVistoriaPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarSolicitacoesManutencaoQuery, List<SolicitacaoManutencaoListaItemDto>>, ListarSolicitacoesManutencaoQueryHandler>();
            services.AddScoped<IQueryHandler<ObterSolicitacaoManutencaoPorIdQuery, SolicitacaoManutencaoDetalheDto?>, ObterSolicitacaoManutencaoPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarEmpreendimentosQuery, List<EmpreendimentoListaItemDto>>, ListarEmpreendimentosQueryHandler>();
            services.AddScoped<IQueryHandler<ObterEmpreendimentoPorIdQuery, EmpreendimentoDetalheDto?>, ObterEmpreendimentoPorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarLotesQuery, List<LoteListaItemDto>>, ListarLotesQueryHandler>();
            services.AddScoped<IQueryHandler<ObterLotePorIdQuery, LoteDetalheDto?>, ObterLotePorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ListarReservasLoteQuery, List<ReservaLoteListaItemDto>>, ListarReservasLoteQueryHandler>();
            services.AddScoped<IQueryHandler<ObterReservaLotePorIdQuery, ReservaLoteDetalheDto?>, ObterReservaLotePorIdQueryHandler>();

            services.AddScoped<IQueryHandler<ObterDashboardQuery, DashboardDto>, ObterDashboardQueryHandler>();

            return services;
        }
    }
}

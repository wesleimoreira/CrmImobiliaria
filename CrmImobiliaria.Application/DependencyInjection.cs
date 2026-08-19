using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Clientes.Commands;
using CrmImobiliaria.Application.Comissoes.Commands;
using CrmImobiliaria.Application.Empreendimentos.Commands;
using CrmImobiliaria.Application.Locacoes.Commands;
using CrmImobiliaria.Application.Lotes.Commands;
using CrmImobiliaria.Application.PagamentosAluguel.Commands;
using CrmImobiliaria.Application.Corretores.Commands;
using CrmImobiliaria.Application.Events;
using CrmImobiliaria.Application.Imoveis.Commands;
using CrmImobiliaria.Application.Leads.Commands;
using CrmImobiliaria.Application.Propostas.Commands;
using CrmImobiliaria.Application.Repasses.Commands;
using CrmImobiliaria.Application.Prestadores.Commands;
using CrmImobiliaria.Application.ReservasLote.Commands;
using CrmImobiliaria.Application.SolicitacoesManutencao.Commands;
using CrmImobiliaria.Application.Vendas.Commands;
using CrmImobiliaria.Application.Visitas.Commands;
using CrmImobiliaria.Application.Vistorias.Commands;
using CrmImobiliaria.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace CrmImobiliaria.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IDomainEventHandler<VendaConcluidaEvent>, VendaConcluidaEventHandler>();
            services.AddScoped<IDomainEventHandler<VendaDistratadaEvent>, VendaDistratadaEventHandler>();
            services.AddScoped<IDomainEventHandler<LocacaoChavesEntreguesEvent>, LocacaoChavesEntreguesEventHandler>();
            services.AddScoped<IDomainEventHandler<LocacaoEncerradaEvent>, LocacaoEncerradaEventHandler>();
            services.AddScoped<IDomainEventHandler<LocacaoCanceladaEvent>, LocacaoCanceladaEventHandler>();

            services.AddScoped<ICommandHandler<CriarClienteCommand, Guid>, CriarClienteCommandHandler>();
            services.AddScoped<ICommandHandler<AtualizarClienteCommand, bool>, AtualizarClienteCommandHandler>();
            services.AddScoped<ICommandHandler<AdicionarPerfilInteresseCommand, bool>, AdicionarPerfilInteresseCommandHandler>();
            services.AddScoped<ICommandHandler<RemoverPerfilInteresseCommand, bool>, RemoverPerfilInteresseCommandHandler>();
            services.AddScoped<ICommandHandler<CriarCorretorCommand, Guid>, CriarCorretorCommandHandler>();
            services.AddScoped<ICommandHandler<AtualizarCorretorCommand, bool>, AtualizarCorretorCommandHandler>();
            services.AddScoped<ICommandHandler<AlterarStatusCorretorCommand, bool>, AlterarStatusCorretorCommandHandler>();

            services.AddScoped<ICommandHandler<CriarImovelCommand, Guid>, CriarImovelCommandHandler>();
            services.AddScoped<ICommandHandler<AtualizarImovelCommand, bool>, AtualizarImovelCommandHandler>();
            services.AddScoped<ICommandHandler<AdicionarAnuncioCommand, Guid>, AdicionarAnuncioCommandHandler>();
            services.AddScoped<ICommandHandler<AlterarStatusAnuncioCommand, bool>, AlterarStatusAnuncioCommandHandler>();

            services.AddScoped<ICommandHandler<CriarLeadCommand, Guid>, CriarLeadCommandHandler>();
            services.AddScoped<ICommandHandler<AvancarEstagioLeadCommand, bool>, AvancarEstagioLeadCommandHandler>();
            services.AddScoped<ICommandHandler<MarcarLeadPerdidoCommand, bool>, MarcarLeadPerdidoCommandHandler>();
            services.AddScoped<ICommandHandler<ReabrirLeadCommand, bool>, ReabrirLeadCommandHandler>();
            services.AddScoped<ICommandHandler<TransferirCorretorLeadCommand, bool>, TransferirCorretorLeadCommandHandler>();

            services.AddScoped<ICommandHandler<AgendarVisitaCommand, Guid>, AgendarVisitaCommandHandler>();
            services.AddScoped<ICommandHandler<ReagendarVisitaCommand, bool>, ReagendarVisitaCommandHandler>();
            services.AddScoped<ICommandHandler<AlterarStatusVisitaCommand, bool>, AlterarStatusVisitaCommandHandler>();

            services.AddScoped<ICommandHandler<CriarPropostaCommand, Guid>, CriarPropostaCommandHandler>();
            services.AddScoped<ICommandHandler<RegistrarContrapropostaCommand, bool>, RegistrarContrapropostaCommandHandler>();
            services.AddScoped<ICommandHandler<AceitarPropostaCommand, bool>, AceitarPropostaCommandHandler>();
            services.AddScoped<ICommandHandler<RecusarPropostaCommand, bool>, RecusarPropostaCommandHandler>();
            services.AddScoped<ICommandHandler<CancelarPropostaCommand, bool>, CancelarPropostaCommandHandler>();

            services.AddScoped<ICommandHandler<FecharVendaCommand, Guid>, FecharVendaCommandHandler>();
            services.AddScoped<ICommandHandler<AnexarContratoVendaCommand, bool>, AnexarContratoVendaCommandHandler>();
            services.AddScoped<ICommandHandler<AtualizarSituacaoDocumentalCommand, bool>, AtualizarSituacaoDocumentalCommandHandler>();
            services.AddScoped<ICommandHandler<DistratarVendaCommand, bool>, DistratarVendaCommandHandler>();

            services.AddScoped<ICommandHandler<CriarRegraComissaoCommand, Guid>, CriarRegraComissaoCommandHandler>();
            services.AddScoped<ICommandHandler<GerarComissaoCommand, Guid>, GerarComissaoCommandHandler>();
            services.AddScoped<ICommandHandler<RegistrarRecebimentoComissaoCommand, bool>, RegistrarRecebimentoComissaoCommandHandler>();
            services.AddScoped<ICommandHandler<RegistrarDistribuicaoComissaoCommand, bool>, RegistrarDistribuicaoComissaoCommandHandler>();

            services.AddScoped<ICommandHandler<IniciarLocacaoCommand, Guid>, IniciarLocacaoCommandHandler>();
            services.AddScoped<ICommandHandler<AvancarEstagioLocacaoCommand, bool>, AvancarEstagioLocacaoCommandHandler>();
            services.AddScoped<ICommandHandler<FormalizarContratoLocacaoCommand, bool>, FormalizarContratoLocacaoCommandHandler>();

            services.AddScoped<ICommandHandler<GerarPagamentoAluguelCommand, Guid>, GerarPagamentoAluguelCommandHandler>();
            services.AddScoped<ICommandHandler<RegistrarPagamentoAluguelCommand, bool>, RegistrarPagamentoAluguelCommandHandler>();

            services.AddScoped<ICommandHandler<GerarRepasseCommand, Guid>, GerarRepasseCommandHandler>();
            services.AddScoped<ICommandHandler<AdicionarDespesaRepasseCommand, bool>, AdicionarDespesaRepasseCommandHandler>();
            services.AddScoped<ICommandHandler<ConfirmarRepasseCommand, bool>, ConfirmarRepasseCommandHandler>();

            services.AddScoped<ICommandHandler<CriarPrestadorCommand, Guid>, CriarPrestadorCommandHandler>();
            services.AddScoped<ICommandHandler<AlterarStatusPrestadorCommand, bool>, AlterarStatusPrestadorCommandHandler>();

            services.AddScoped<ICommandHandler<AgendarVistoriaCommand, Guid>, AgendarVistoriaCommandHandler>();
            services.AddScoped<ICommandHandler<RegistrarRealizacaoVistoriaCommand, bool>, RegistrarRealizacaoVistoriaCommandHandler>();
            services.AddScoped<ICommandHandler<CancelarVistoriaCommand, bool>, CancelarVistoriaCommandHandler>();

            services.AddScoped<ICommandHandler<AbrirSolicitacaoManutencaoCommand, Guid>, AbrirSolicitacaoManutencaoCommandHandler>();
            services.AddScoped<ICommandHandler<AdicionarOrcamentoCommand, Guid>, AdicionarOrcamentoCommandHandler>();
            services.AddScoped<ICommandHandler<AprovarOrcamentoCommand, bool>, AprovarOrcamentoCommandHandler>();
            services.AddScoped<ICommandHandler<IniciarExecucaoSolicitacaoCommand, bool>, IniciarExecucaoSolicitacaoCommandHandler>();
            services.AddScoped<ICommandHandler<ConcluirServicoCommand, bool>, ConcluirServicoCommandHandler>();
            services.AddScoped<ICommandHandler<CancelarSolicitacaoCommand, bool>, CancelarSolicitacaoCommandHandler>();

            services.AddScoped<ICommandHandler<CriarEmpreendimentoCommand, Guid>, CriarEmpreendimentoCommandHandler>();
            services.AddScoped<ICommandHandler<AvancarStatusEmpreendimentoCommand, bool>, AvancarStatusEmpreendimentoCommandHandler>();
            services.AddScoped<ICommandHandler<DefinirCampanhaEmpreendimentoCommand, bool>, DefinirCampanhaEmpreendimentoCommandHandler>();

            services.AddScoped<ICommandHandler<CriarLoteCommand, Guid>, CriarLoteCommandHandler>();
            services.AddScoped<ICommandHandler<AvancarStatusLoteCommand, bool>, AvancarStatusLoteCommandHandler>();

            services.AddScoped<ICommandHandler<CriarReservaLoteCommand, Guid>, CriarReservaLoteCommandHandler>();
            services.AddScoped<ICommandHandler<ExpirarReservaLoteCommand, bool>, ExpirarReservaLoteCommandHandler>();
            services.AddScoped<ICommandHandler<ConverterReservaLoteCommand, bool>, ConverterReservaLoteCommandHandler>();
            services.AddScoped<ICommandHandler<CancelarReservaLoteCommand, bool>, CancelarReservaLoteCommandHandler>();

            return services;
        }
    }
}

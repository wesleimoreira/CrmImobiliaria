using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence
{
    public sealed class CrmDbContext(DbContextOptions<CrmDbContext> options, IDomainEventDispatcher domainEventDispatcher)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Corretor> Corretores => Set<Corretor>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<Imovel> Imoveis => Set<Imovel>();
        public DbSet<Visita> Visitas => Set<Visita>();
        public DbSet<Proposta> Propostas => Set<Proposta>();
        public DbSet<Venda> Vendas => Set<Venda>();
        public DbSet<RegraComissao> RegrasComissao => Set<RegraComissao>();
        public DbSet<Comissao> Comissoes => Set<Comissao>();
        public DbSet<Locacao> Locacoes => Set<Locacao>();
        public DbSet<PagamentoAluguel> PagamentosAluguel => Set<PagamentoAluguel>();
        public DbSet<Repasse> Repasses => Set<Repasse>();
        public DbSet<Vistoria> Vistorias => Set<Vistoria>();
        public DbSet<SolicitacaoManutencao> SolicitacoesManutencao => Set<SolicitacaoManutencao>();
        public DbSet<Prestador> Prestadores => Set<Prestador>();
        public DbSet<Empreendimento> Empreendimentos => Set<Empreendimento>();
        public DbSet<Lote> Lotes => Set<Lote>();
        public DbSet<ReservaLote> ReservasLote => Set<ReservaLote>();
        public DbSet<ReservaTemporada> ReservasTemporada => Set<ReservaTemporada>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrmDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DespacharEventosDominioAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        // Despacha os eventos de domínio ANTES de persistir, para que as mutações que os handlers
        // fazem em outros agregados (ex: AnuncioImovel.Fechar() quando uma Venda é concluída) entrem
        // na mesma chamada de SaveChanges/transação que a mudança que disparou o evento.
        private async Task DespacharEventosDominioAsync(CancellationToken cancellationToken)
        {
            for (var iteracao = 0; iteracao < 10; iteracao++)   // limite de segurança contra loop infinito
            {
                var agregados = ChangeTracker.Entries<AggregateRoot>()
                    .Select(e => e.Entity)
                    .Where(a => a.EventosDominio.Count > 0)
                    .ToList();

                if (agregados.Count == 0) break;

                var eventos = agregados.SelectMany(a => a.EventosDominio).ToList();
                foreach (var agregado in agregados) agregado.LimparEventosDominio();

                foreach (var evento in eventos)
                    await domainEventDispatcher.DispatchAsync(evento, cancellationToken);

                ChangeTracker.DetectChanges();   // pega mutações que os handlers acabaram de fazer em outros agregados
            }
        }
    }
}

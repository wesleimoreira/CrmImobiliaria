using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class PropostaConfiguration : IEntityTypeConfiguration<Proposta>
    {
        public void Configure(EntityTypeBuilder<Proposta> b)
        {
            b.ConfigurarAgregado();
            b.Property(p => p.ValorAnunciado).HasConversion(Conversoes.DinheiroConverter);
            b.Property(p => p.ValorProposto).HasConversion(Conversoes.DinheiroConverter);
            b.Property(p => p.Entrada).HasConversion(Conversoes.DinheiroNuloConverter);
            b.Property(p => p.ValorParcela).HasConversion(Conversoes.DinheiroNuloConverter);
            b.Property(p => p.MotivoRecusa).HasMaxLength(1000);

            b.OwnsMany(p => p.Historico, hb =>
            {
                hb.ToTable("PropostaHistoricoNegociacoes");
                hb.Property(h => h.Valor).HasConversion(Conversoes.DinheiroConverter);
                hb.Property(h => h.Observacao).HasMaxLength(500);
            });
        }
    }
}

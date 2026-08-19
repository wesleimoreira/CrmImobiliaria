using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class ComissaoConfiguration : IEntityTypeConfiguration<Comissao>
    {
        public void Configure(EntityTypeBuilder<Comissao> b)
        {
            b.ConfigurarAgregado();
            b.Property(c => c.ValorBase).HasConversion(Conversoes.DinheiroConverter);
            b.Property(c => c.PercentualComissao).HasConversion(Conversoes.PercentualConverter);
            b.Property(c => c.ComissaoTotal).HasConversion(Conversoes.DinheiroConverter);
            b.Property(c => c.ComissaoRecebida).HasConversion(Conversoes.DinheiroConverter);
            b.Property(c => c.ComissaoDistribuida).HasConversion(Conversoes.DinheiroConverter);

            b.OwnsMany(c => c.Rateio, rb =>
            {
                rb.ToTable("ComissaoRateios");
                rb.Property(r => r.Percentual).HasConversion(Conversoes.PercentualConverter);
                rb.Property(r => r.Valor).HasConversion(Conversoes.DinheiroConverter);
            });
        }
    }
}

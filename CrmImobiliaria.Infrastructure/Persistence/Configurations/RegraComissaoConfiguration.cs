using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class RegraComissaoConfiguration : IEntityTypeConfiguration<RegraComissao>
    {
        public void Configure(EntityTypeBuilder<RegraComissao> b)
        {
            b.ConfigurarAgregado();
            b.Property(r => r.Nome).HasMaxLength(200).IsRequired();
            b.Property(r => r.PercentualComissaoTotal).HasConversion(Conversoes.PercentualConverter);

            b.OwnsMany(r => r.Rateio, ib =>
            {
                ib.ToTable("RegraComissaoItensRateio");
                ib.Property(i => i.Percentual).HasConversion(Conversoes.PercentualConverter);
            });
        }
    }
}

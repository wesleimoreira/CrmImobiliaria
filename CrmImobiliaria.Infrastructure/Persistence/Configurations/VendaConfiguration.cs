using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class VendaConfiguration : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> b)
        {
            b.ConfigurarAgregado();
            b.Property(v => v.ValorFinal).HasConversion(Conversoes.DinheiroConverter);
            b.Property(v => v.NumeroContrato).HasMaxLength(50);
            b.Property(v => v.UrlContrato).HasMaxLength(500);
            b.Property(v => v.MotivoDistrato).HasMaxLength(1000);

            b.OwnsOne(v => v.Financiamento, fb =>
            {
                fb.Property(f => f.Banco).HasMaxLength(200).IsRequired();
                fb.Property(f => f.ValorFinanciado).HasConversion(Conversoes.DinheiroConverter);
            });
        }
    }
}

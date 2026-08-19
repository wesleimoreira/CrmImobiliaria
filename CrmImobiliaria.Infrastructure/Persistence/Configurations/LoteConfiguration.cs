using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class LoteConfiguration : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> b)
        {
            b.ConfigurarAgregado();
            b.Property(l => l.Quadra).HasMaxLength(20).IsRequired();
            b.Property(l => l.Numero).HasMaxLength(20).IsRequired();
            b.Property(l => l.Area).HasConversion(Conversoes.AreaConverter);
            b.Property(l => l.Valor).HasConversion(Conversoes.DinheiroConverter);
            b.Property(l => l.EntradaMinima).HasConversion(Conversoes.DinheiroConverter);
            b.Property(l => l.ValorPromocional).HasConversion(Conversoes.DinheiroNuloConverter);
        }
    }
}

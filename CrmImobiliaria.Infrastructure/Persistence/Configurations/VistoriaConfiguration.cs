using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class VistoriaConfiguration : IEntityTypeConfiguration<Vistoria>
    {
        public void Configure(EntityTypeBuilder<Vistoria> b)
        {
            b.ConfigurarAgregado();
            b.Property(v => v.Observacoes).HasMaxLength(1000);

            b.Property(v => v.Fotos)
                .HasField("_fotos")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyList<string>>(
                    (a, c) => a!.SequenceEqual(c!),
                    v => v.Aggregate(0, (h, s) => HashCode.Combine(h, s)),
                    v => v.ToList()));
        }
    }
}

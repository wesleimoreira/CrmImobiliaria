using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> b)
        {
            b.ConfigurarAgregado();
            b.Property(l => l.MotivoPerda).HasMaxLength(1000);

            b.Property(l => l.ImoveisApresentados)
                .HasField("_imoveisApresentados")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList())
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyList<Guid>>(
                    (a, c) => a!.SequenceEqual(c!),
                    v => v.Aggregate(0, (h, g) => HashCode.Combine(h, g)),
                    v => v.ToList()));

            b.OwnsMany(l => l.Historico, hb =>
            {
                hb.ToTable("LeadHistoricoEstagios");
                hb.Property(h => h.Estagio).HasConversion<string>().HasMaxLength(30);
                hb.Property(h => h.Observacao).HasMaxLength(500);
            });
        }
    }
}

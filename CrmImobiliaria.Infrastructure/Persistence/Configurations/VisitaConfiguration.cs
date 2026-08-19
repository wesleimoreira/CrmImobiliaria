using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class VisitaConfiguration : IEntityTypeConfiguration<Visita>
    {
        public void Configure(EntityTypeBuilder<Visita> b)
        {
            b.ConfigurarAgregado();
            b.Property(v => v.Feedback).HasMaxLength(2000);
        }
    }
}

using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class ReservaLoteConfiguration : IEntityTypeConfiguration<ReservaLote>
    {
        public void Configure(EntityTypeBuilder<ReservaLote> b)
        {
            b.ConfigurarAgregado();
        }
    }
}

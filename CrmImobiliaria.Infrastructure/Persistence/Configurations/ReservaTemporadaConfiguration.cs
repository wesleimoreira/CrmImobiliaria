using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class ReservaTemporadaConfiguration : IEntityTypeConfiguration<ReservaTemporada>
    {
        public void Configure(EntityTypeBuilder<ReservaTemporada> b)
        {
            b.ConfigurarAgregado();
            b.Property(r => r.ValorDiaria).HasConversion(Conversoes.DinheiroConverter);
            b.Property(r => r.ValorTotal).HasConversion(Conversoes.DinheiroConverter);
            b.Property(r => r.Observacoes).HasMaxLength(1000);

            b.OwnsOne(r => r.Periodo, pb =>
            {
                pb.Property(p => p.DataInicial);
                pb.Property(p => p.DataFinal);
            });
        }
    }
}

using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class PagamentoAluguelConfiguration : IEntityTypeConfiguration<PagamentoAluguel>
    {
        public void Configure(EntityTypeBuilder<PagamentoAluguel> b)
        {
            b.ConfigurarAgregado();
            b.Property(p => p.Competencia).HasConversion(Conversoes.CompetenciaConverter);
            b.Property(p => p.ValorDevido).HasConversion(Conversoes.DinheiroConverter);
            b.Property(p => p.ValorPago).HasConversion(Conversoes.DinheiroNuloConverter);
        }
    }
}

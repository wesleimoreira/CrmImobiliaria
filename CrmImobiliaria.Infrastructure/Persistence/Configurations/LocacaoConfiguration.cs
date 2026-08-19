using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(EntityTypeBuilder<Locacao> b)
        {
            b.ConfigurarAgregado();
            b.Property(l => l.ValorAluguel).HasConversion(Conversoes.DinheiroNuloConverter);
            b.Property(l => l.Garantia).HasConversion(Conversoes.DinheiroNuloConverter);
            b.Property(l => l.TaxaAdministracao).HasConversion(Conversoes.PercentualNuloConverter);
            b.Property(l => l.MotivoReprovacaoOuCancelamento).HasMaxLength(1000);

            b.OwnsOne(l => l.Periodo, pb =>
            {
                pb.Property(p => p.DataInicial);
                pb.Property(p => p.DataFinal);
            });
        }
    }
}

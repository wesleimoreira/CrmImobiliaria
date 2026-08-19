using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class RepasseConfiguration : IEntityTypeConfiguration<Repasse>
    {
        public void Configure(EntityTypeBuilder<Repasse> b)
        {
            b.ConfigurarAgregado();
            b.Property(r => r.Competencia).HasConversion(Conversoes.CompetenciaConverter);
            b.Property(r => r.ValorAluguelRecebido).HasConversion(Conversoes.DinheiroConverter);
            b.Property(r => r.TaxaAdministracao).HasConversion(Conversoes.PercentualConverter);
            b.Property(r => r.ValorTaxaAdministracao).HasConversion(Conversoes.DinheiroConverter);
            b.Property(r => r.ValorLiquido).HasConversion(Conversoes.DinheiroConverter);
            b.Property(r => r.Comprovante).HasMaxLength(500);

            b.OwnsMany(r => r.Despesas, db =>
            {
                db.ToTable("RepasseDespesas");
                db.Property(d => d.Descricao).HasMaxLength(200).IsRequired();
                db.Property(d => d.Valor).HasConversion(Conversoes.DinheiroConverter);
            });
        }
    }
}

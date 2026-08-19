using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class EmpreendimentoConfiguration : IEntityTypeConfiguration<Empreendimento>
    {
        public void Configure(EntityTypeBuilder<Empreendimento> b)
        {
            b.ConfigurarAgregado();
            b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
            b.Property(e => e.LoteadoraIncorporadora).HasMaxLength(200).IsRequired();
            b.Property(e => e.PercentualComissao).HasConversion(Conversoes.PercentualConverter);
            b.Property(e => e.CampanhaVigente).HasMaxLength(200);

            b.OwnsOne(e => e.Localizacao, lb =>
            {
                lb.Property(x => x.Logradouro).HasMaxLength(200).IsRequired();
                lb.Property(x => x.Numero).HasMaxLength(20).IsRequired();
                lb.Property(x => x.Complemento).HasMaxLength(100);
                lb.Property(x => x.Bairro).HasMaxLength(100).IsRequired();
                lb.Property(x => x.Cidade).HasMaxLength(100).IsRequired();
                lb.Property(x => x.Uf).HasMaxLength(2).IsRequired();
                lb.Property(x => x.Cep).HasMaxLength(8).IsRequired();
            });
        }
    }
}

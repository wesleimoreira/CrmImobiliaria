using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class SolicitacaoManutencaoConfiguration : IEntityTypeConfiguration<SolicitacaoManutencao>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoManutencao> b)
        {
            b.ConfigurarAgregado();
            b.Property(s => s.Descricao).HasMaxLength(2000).IsRequired();

            b.OwnsMany(s => s.Orcamentos, ob =>
            {
                ob.ToTable("Orcamentos");
                ob.WithOwner().HasForeignKey("SolicitacaoManutencaoId");
                ob.HasKey(o => o.Id);
                ob.Property(o => o.Id).ValueGeneratedNever();
                ob.Property(o => o.Valor).HasConversion(Conversoes.DinheiroConverter);
                ob.Property(o => o.Descricao).HasMaxLength(1000);
            });
        }
    }
}

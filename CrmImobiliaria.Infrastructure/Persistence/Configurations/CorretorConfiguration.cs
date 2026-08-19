using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class CorretorConfiguration : IEntityTypeConfiguration<Corretor>
    {
        public void Configure(EntityTypeBuilder<Corretor> b)
        {
            b.ConfigurarAgregado();
            b.Property(c => c.Nome).HasMaxLength(200).IsRequired();
            b.Property(c => c.Creci).HasConversion(Conversoes.CreciConverter).HasMaxLength(20);
            b.HasIndex(c => c.Creci).IsUnique();
            b.Property(c => c.Telefone).HasConversion(Conversoes.TelefoneConverter).HasMaxLength(11);
            b.Property(c => c.Email).HasConversion(Conversoes.EmailConverter).HasMaxLength(256);
            b.Property(c => c.Equipe).HasMaxLength(100);
        }
    }
}

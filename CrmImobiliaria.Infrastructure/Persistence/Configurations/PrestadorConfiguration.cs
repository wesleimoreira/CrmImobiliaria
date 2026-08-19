using CrmImobiliaria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class PrestadorConfiguration : IEntityTypeConfiguration<Prestador>
    {
        public void Configure(EntityTypeBuilder<Prestador> b)
        {
            b.ConfigurarAgregado();
            b.Property(p => p.Nome).HasMaxLength(200).IsRequired();
            b.Property(p => p.Telefone).HasConversion(Conversoes.TelefoneConverter).HasMaxLength(11);
            b.Property(p => p.Email).HasConversion(Conversoes.EmailNuloConverter).HasMaxLength(256);
            b.Property(p => p.Especialidade).HasMaxLength(200);
        }
    }
}

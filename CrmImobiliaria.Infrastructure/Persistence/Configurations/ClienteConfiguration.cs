using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> b)
        {
            b.ConfigurarAgregado();
            b.Property(c => c.Nome).HasMaxLength(200).IsRequired();
            b.Property(c => c.Documento).HasConversion(Conversoes.CpfCnpjNuloConverter).HasMaxLength(14);
            b.HasIndex(c => c.Documento).IsUnique().HasFilter("[Documento] IS NOT NULL");
            b.Property(c => c.Telefone).HasConversion(Conversoes.TelefoneConverter).HasMaxLength(11);
            b.Property(c => c.WhatsApp).HasConversion(Conversoes.TelefoneNuloConverter).HasMaxLength(11);
            b.Property(c => c.Email).HasConversion(Conversoes.EmailConverter).HasMaxLength(256);
            b.Property(c => c.CampanhaEspecifica).HasMaxLength(200);
            b.Property(c => c.Observacoes).HasMaxLength(2000);

            b.Property(c => c.Tipos)
                .HasField("_tipos")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    v => string.Join(',', v.Select(t => (int)t)),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => (TipoCliente)int.Parse(s)).ToHashSet())
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlySet<TipoCliente>>(
                    (a, c) => a!.SetEquals(c!),
                    v => v.Aggregate(0, (h, t) => HashCode.Combine(h, t)),
                    v => v.ToHashSet()));

            b.OwnsMany(c => c.PerfisInteresse, pb =>
            {
                pb.ToTable("PerfisInteresse");
                pb.WithOwner().HasForeignKey("ClienteId");
                pb.HasKey(p => p.Id);
                pb.Property(p => p.Id).ValueGeneratedNever();
                pb.Property(p => p.LocalizacaoDesejada).HasMaxLength(200);
                pb.Property(p => p.Observacoes).HasMaxLength(1000);

                pb.OwnsOne(p => p.FaixaPreco, fb =>
                {
                    fb.Property(f => f.Minimo).HasConversion(Conversoes.DinheiroConverter);
                    fb.Property(f => f.Maximo).HasConversion(Conversoes.DinheiroConverter);
                });
            });
        }
    }
}

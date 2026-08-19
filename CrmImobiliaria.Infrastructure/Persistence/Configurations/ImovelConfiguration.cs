using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    public sealed class ImovelConfiguration : IEntityTypeConfiguration<Imovel>
    {
        public void Configure(EntityTypeBuilder<Imovel> b)
        {
            b.ConfigurarAgregado();
            b.Property(i => i.Area).HasConversion(Conversoes.AreaConverter);

            b.OwnsOne(i => i.Endereco, eb =>
            {
                eb.Property(x => x.Logradouro).HasMaxLength(200).IsRequired();
                eb.Property(x => x.Numero).HasMaxLength(20).IsRequired();
                eb.Property(x => x.Complemento).HasMaxLength(100);
                eb.Property(x => x.Bairro).HasMaxLength(100).IsRequired();
                eb.Property(x => x.Cidade).HasMaxLength(100).IsRequired();
                eb.Property(x => x.Uf).HasMaxLength(2).IsRequired();
                eb.Property(x => x.Cep).HasMaxLength(8).IsRequired();
            });

            MapearListaDeTextos(b, i => i.Caracteristicas, "_caracteristicas");
            MapearListaDeTextos(b, i => i.Fotos, "_fotos");
            MapearListaDeTextos(b, i => i.Documentos, "_documentos");

            b.OwnsMany(i => i.Anuncios, ab =>
            {
                ab.ToTable("Anuncios");
                ab.WithOwner().HasForeignKey(a => a.ImovelId);
                ab.HasKey(a => a.Id);
                ab.Property(a => a.Id).ValueGeneratedNever();
                ab.Property(a => a.Codigo).HasConversion(Conversoes.CodigoImovelConverter).HasMaxLength(20);
                ab.HasIndex(a => a.Codigo).IsUnique();
                ab.Property(a => a.Valor).HasConversion(Conversoes.DinheiroConverter);

                // RegraEstadia tem parâmetros de construtor (diasCheckin/diasCheckout) com nomes que não
                // batem com as propriedades (DiasCheckinPermitidos/DiasCheckoutPermitidos), então o EF não
                // consegue fazer constructor binding via OwnsOne. Serializa o VO inteiro numa única coluna
                // e reconstrói via RegraEstadia.Criar(...), igual ao padrão usado para Codigo/Creci.
                ab.Property(a => a.RegraEstadia).HasConversion(RegraEstadiaConverter).HasMaxLength(100);
            });
        }

        private static readonly ValueConverter<RegraEstadia?, string?> RegraEstadiaConverter = new(
            v => v == null
                ? null
                : $"{v.EstadiaMinimaNoites}|{string.Join(',', v.DiasCheckinPermitidos.Select(d => (int)d))}|{string.Join(',', v.DiasCheckoutPermitidos.Select(d => (int)d))}",
            v => v == null ? null : ParseRegraEstadia(v));

        private static RegraEstadia ParseRegraEstadia(string texto)
        {
            var partes = texto.Split('|');
            var checkin = partes[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => (DayOfWeek)int.Parse(s));
            var checkout = partes[2].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => (DayOfWeek)int.Parse(s));
            return RegraEstadia.Criar(int.Parse(partes[0]), checkin, checkout).Value!;
        }

        // Padrão B: List<string> exposto só como IReadOnlyList, backed por campo privado.
        private static void MapearListaDeTextos(EntityTypeBuilder<Imovel> b, System.Linq.Expressions.Expression<Func<Imovel, IReadOnlyList<string>>> propriedade, string nomeCampo)
        {
            b.Property(propriedade)
                .HasField(nomeCampo)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    v => string.Join('|', v),
                    v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyList<string>>(
                    (a, c) => a!.SequenceEqual(c!),
                    v => v.Aggregate(0, (h, s) => HashCode.Combine(h, s)),
                    v => v.ToList()));
        }
    }
}

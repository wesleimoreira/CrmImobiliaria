using CrmImobiliaria.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrmImobiliaria.Infrastructure.Persistence.Configurations
{
    internal static class EntityConfigurationExtensions
    {
        public static void ConfigurarEntidade<T>(this EntityTypeBuilder<T> builder) where T : Entity
        {
            builder.HasKey(e => e.Id);
            // Guid.CreateVersion7() já vem setado do construtor do domínio — nunca deixar o banco gerar.
            builder.Property(e => e.Id).ValueGeneratedNever();
        }

        public static void ConfigurarAgregado<T>(this EntityTypeBuilder<T> builder) where T : AggregateRoot
        {
            builder.ConfigurarEntidade();
            builder.Ignore(a => a.EventosDominio);
        }
    }
}

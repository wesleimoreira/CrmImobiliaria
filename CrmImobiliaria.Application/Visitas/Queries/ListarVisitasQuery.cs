using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Visitas.Queries
{
    public sealed record ListarVisitasQuery(string? Termo) : IQuery<List<VisitaListaItemDto>>;

    public sealed record VisitaListaItemDto(
        Guid Id, string ClienteNome, string ImovelEndereco, string CorretorNome, DateTime DataHora, StatusVisita Status);
}

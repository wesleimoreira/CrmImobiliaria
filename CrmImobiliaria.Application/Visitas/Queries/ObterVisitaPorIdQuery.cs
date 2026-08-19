using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Visitas.Queries
{
    public sealed record ObterVisitaPorIdQuery(Guid Id) : IQuery<VisitaDetalheDto?>;

    public sealed record VisitaDetalheDto(
        Guid Id, string ClienteNome, string ImovelEndereco, string CorretorNome,
        DateTime DataHora, StatusVisita Status, string? Feedback);
}

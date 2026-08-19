using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Corretores.Queries
{
    public sealed record ObterCorretorPorIdQuery(Guid Id) : IQuery<CorretorDetalheDto?>;

    public sealed record CorretorDetalheDto(
        Guid Id, string Nome, string Creci, string Telefone, string Email,
        string? Equipe, Guid? GerenteId, StatusCorretor Status, DateTime CriadoEm);
}

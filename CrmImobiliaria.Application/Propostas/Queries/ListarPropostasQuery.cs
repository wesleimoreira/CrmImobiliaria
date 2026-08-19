using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Propostas.Queries
{
    public sealed record ListarPropostasQuery(string? Termo) : IQuery<List<PropostaListaItemDto>>;

    public sealed record PropostaListaItemDto(
        Guid Id, string ClienteNome, string ImovelEndereco, decimal ValorProposto, StatusProposta Status, DateTime CriadoEm);
}

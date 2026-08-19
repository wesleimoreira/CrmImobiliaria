using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Locacoes.Queries
{
    public sealed record ListarLocacoesQuery(string? Termo) : IQuery<List<LocacaoListaItemDto>>;

    public sealed record LocacaoListaItemDto(
        Guid Id, Guid ImovelId, string LocatarioNome, string ImovelEndereco, EstagioLocacao EstagioAtual, StatusLocacao Status, decimal? ValorAluguel);
}

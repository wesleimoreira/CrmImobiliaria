using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Comissoes.Queries
{
    public sealed record ListarRegrasComissaoQuery : IQuery<List<RegraComissaoListaItemDto>>;

    public sealed record RegraComissaoListaItemDto(Guid Id, string Nome, decimal PercentualComissaoTotal, string? ImovelEndereco, int QuantidadeItensRateio);
}

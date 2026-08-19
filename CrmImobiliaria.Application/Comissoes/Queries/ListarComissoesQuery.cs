using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Comissoes.Queries
{
    public sealed record ListarComissoesQuery : IQuery<List<ComissaoListaItemDto>>;

    public sealed record ComissaoListaItemDto(
        Guid Id, OrigemComissao OrigemTipo, decimal ValorBase, decimal ComissaoTotal, decimal ComissaoRecebida, decimal ComissaoDistribuida);
}

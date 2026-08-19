using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Repasses.Queries
{
    public sealed record ListarRepassesQuery(Guid? LocacaoId = null) : IQuery<List<RepasseListaItemDto>>;

    public sealed record RepasseListaItemDto(
        Guid Id, Guid LocacaoId, string Imovel, string Proprietario, int Mes, int Ano,
        decimal ValorAluguelRecebido, decimal ValorLiquido, StatusRepasse Status);
}

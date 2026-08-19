using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Lotes.Queries
{
    public sealed record ListarLotesQuery(Guid? EmpreendimentoId = null) : IQuery<List<LoteListaItemDto>>;

    public sealed record LoteListaItemDto(
        Guid Id, Guid EmpreendimentoId, string EmpreendimentoNome, string Quadra, string Numero,
        decimal AreaM2, decimal ValorVigente, StatusLote Status);
}

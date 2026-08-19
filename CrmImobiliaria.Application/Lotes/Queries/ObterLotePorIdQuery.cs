using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Lotes.Queries
{
    public sealed record ObterLotePorIdQuery(Guid Id) : IQuery<LoteDetalheDto?>;

    public sealed record LoteDetalheDto(
        Guid Id, Guid EmpreendimentoId, string EmpreendimentoNome, string Quadra, string Numero, decimal AreaM2,
        decimal Valor, decimal EntradaMinima, int ParcelamentoMaximo, decimal? ValorPromocional, decimal ValorVigente, StatusLote Status);
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Empreendimentos.Queries
{
    public sealed record ObterEmpreendimentoPorIdQuery(Guid Id) : IQuery<EmpreendimentoDetalheDto?>;

    public sealed record EmpreendimentoDetalheDto(
        Guid Id, string Nome, string LoteadoraIncorporadora, string Localizacao, DateOnly? DataLancamento,
        int TotalLotes, int NumeroQuadras, decimal PercentualComissao, string? CampanhaVigente, StatusEmpreendimento Status);
}

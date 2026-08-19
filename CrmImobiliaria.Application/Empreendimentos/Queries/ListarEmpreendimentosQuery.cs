using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Empreendimentos.Queries
{
    public sealed record ListarEmpreendimentosQuery : IQuery<List<EmpreendimentoListaItemDto>>;

    public sealed record EmpreendimentoListaItemDto(
        Guid Id, string Nome, string LoteadoraIncorporadora, string Localizacao, int TotalLotes, int NumeroQuadras, StatusEmpreendimento Status);
}

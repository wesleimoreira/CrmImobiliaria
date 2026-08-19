using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Domain.Events
{
    // Levantado quando uma Venda é distratada. A Application usa esses IDs para reabrir o
    // AnuncioImovel (StatusAnuncio.Disponivel via Reabrir()) quando é venda de imóvel avulso.
    // Lote não é tocado automaticamente: reverter um lote vendido fica pra uma ação explícita
    // futura (Lote.Distratar exige um motivo, que este evento não carrega).
    public sealed record VendaDistratadaEvent(
        Guid VendaId,
        Guid? ImovelId,
        Guid? AnuncioImovelId,
        Guid? LoteId,
        DateTime OcorridoEm) : IDomainEvent;
}

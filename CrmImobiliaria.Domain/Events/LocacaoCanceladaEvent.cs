using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Domain.Events
{
    // Levantado quando uma Locacao é cancelada antes de ficar ativa. A Application usa o
    // AnuncioImovelId para reabrir o anúncio (StatusAnuncio.Disponivel via CancelarNegociacao()
    // ou Reabrir(), dependendo do status em que o anúncio estiver).
    public sealed record LocacaoCanceladaEvent(
        Guid LocacaoId,
        Guid AnuncioImovelId,
        DateTime OcorridoEm) : IDomainEvent;
}

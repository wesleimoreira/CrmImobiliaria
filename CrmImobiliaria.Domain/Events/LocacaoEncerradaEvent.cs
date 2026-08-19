using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Domain.Events
{
    // Levantado quando uma Locacao ativa é encerrada. A Application usa o AnuncioImovelId
    // para reabrir o anúncio (StatusAnuncio.Disponivel via Reabrir()).
    public sealed record LocacaoEncerradaEvent(
        Guid LocacaoId,
        Guid AnuncioImovelId,
        DateTime OcorridoEm) : IDomainEvent;
}

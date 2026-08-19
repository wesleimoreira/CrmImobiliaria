using CrmImobiliaria.Domain.Common;

namespace CrmImobiliaria.Domain.Events
{
    // Levantado quando as chaves de uma Locacao são entregues (Status -> Ativa).
    // A Application usa o AnuncioImovelId para fechar o anúncio (StatusAnuncio.Alugado).
    public sealed record LocacaoChavesEntreguesEvent(
        Guid LocacaoId,
        Guid AnuncioImovelId,
        DateTime OcorridoEm) : IDomainEvent;
}

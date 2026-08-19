using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Entities
{
    // Snapshot do valor de cada papel no momento em que a comissão foi gerada.
    public sealed record RateioComissao(PapelComissao Papel, Percentual Percentual, Dinheiro Valor, Guid? BeneficiarioId = null);
}

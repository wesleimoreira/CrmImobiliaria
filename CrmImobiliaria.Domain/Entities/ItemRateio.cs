using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed record ItemRateio(PapelComissao Papel, Percentual Percentual);
}

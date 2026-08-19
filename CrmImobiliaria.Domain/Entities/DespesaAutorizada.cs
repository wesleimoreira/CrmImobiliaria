using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Entities
{
    public sealed record DespesaAutorizada(string Descricao, Dinheiro Valor);
}

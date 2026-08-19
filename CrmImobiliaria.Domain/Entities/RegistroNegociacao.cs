using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Entities
{
    // Snapshot imutável de cada rodada de negociação — mesmo padrão do RegistroEstagio do Lead.
    public sealed record RegistroNegociacao(Dinheiro Valor, OrigemNegociacao Origem, DateTime DataHora, string? Observacao = null);
}

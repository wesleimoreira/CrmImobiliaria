using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Domain.Entities
{
    // Snapshot imutável de uma transição — só para histórico/auditoria,
    // por isso é um record simples, não um Value Object de regra.
    public sealed record RegistroEstagio(EstagioFunil Estagio, DateTime DataHora, string? Observacao = null);
}

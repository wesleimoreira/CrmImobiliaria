using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Leads.Queries
{
    public sealed record ObterLeadPorIdQuery(Guid Id) : IQuery<LeadDetalheDto?>;

    public sealed record LeadDetalheDto(
        Guid Id, Guid ClienteId, string ClienteNome, Guid CorretorId, string CorretorNome,
        EstagioFunil EstagioAtual, string? MotivoPerda, DateTime CriadoEm, List<HistoricoItemDto> Historico);

    public sealed record HistoricoItemDto(EstagioFunil Estagio, DateTime DataHora, string? Observacao);
}

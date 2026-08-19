using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Leads.Queries
{
    public sealed record ListarLeadsQuery(string? Termo) : IQuery<List<LeadListaItemDto>>;

    public sealed record LeadListaItemDto(Guid Id, string ClienteNome, string CorretorNome, EstagioFunil EstagioAtual, DateTime CriadoEm);
}

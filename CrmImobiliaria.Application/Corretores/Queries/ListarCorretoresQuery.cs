using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Corretores.Queries
{
    public sealed record ListarCorretoresQuery : IQuery<List<CorretorResumoDto>>;

    public sealed record CorretorResumoDto(Guid Id, string Nome, string Creci, string Telefone, string Email, string Status);
}

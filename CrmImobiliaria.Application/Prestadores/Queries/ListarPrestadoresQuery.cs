using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Prestadores.Queries
{
    public sealed record ListarPrestadoresQuery : IQuery<List<PrestadorListaItemDto>>;

    public sealed record PrestadorListaItemDto(Guid Id, string Nome, string Telefone, string? Email, string? Especialidade, bool Ativo);
}

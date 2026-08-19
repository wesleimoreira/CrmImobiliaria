using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Clientes.Queries
{
    public sealed record ListarClientesQuery(string? Termo) : IQuery<List<ClienteListaItemDto>>;

    public sealed record ClienteListaItemDto(Guid Id, string Nome, string Telefone, string Email, string Tipos, string CorretorNome);
}

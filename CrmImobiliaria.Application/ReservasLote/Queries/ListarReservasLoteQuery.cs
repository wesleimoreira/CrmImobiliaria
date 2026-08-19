using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.ReservasLote.Queries
{
    public sealed record ListarReservasLoteQuery : IQuery<List<ReservaLoteListaItemDto>>;

    public sealed record ReservaLoteListaItemDto(
        Guid Id, string LoteDescricao, string ClienteNome, string CorretorNome,
        DateOnly DataReserva, DateOnly DataExpiracao, StatusReservaLote Status);
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.ReservasLote.Queries
{
    public sealed record ObterReservaLotePorIdQuery(Guid Id) : IQuery<ReservaLoteDetalheDto?>;

    public sealed record ReservaLoteDetalheDto(
        Guid Id, Guid LoteId, string LoteDescricao, string ClienteNome, string CorretorNome,
        DateOnly DataReserva, DateOnly DataExpiracao, StatusReservaLote Status);
}

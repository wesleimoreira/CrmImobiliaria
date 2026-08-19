using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vistorias.Queries
{
    public sealed record ListarVistoriasQuery : IQuery<List<VistoriaListaItemDto>>;

    public sealed record VistoriaListaItemDto(
        Guid Id, string ImovelEndereco, TipoVistoria Tipo, DateTime DataAgendada, DateTime? DataRealizada,
        string ResponsavelNome, StatusVistoria Status);
}

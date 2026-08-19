using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vistorias.Queries
{
    public sealed record ObterVistoriaPorIdQuery(Guid Id) : IQuery<VistoriaDetalheDto?>;

    public sealed record VistoriaDetalheDto(
        Guid Id, string ImovelEndereco, TipoVistoria Tipo, DateTime DataAgendada, DateTime? DataRealizada,
        string ResponsavelNome, StatusVistoria Status, string? Observacoes, List<string> Fotos);
}

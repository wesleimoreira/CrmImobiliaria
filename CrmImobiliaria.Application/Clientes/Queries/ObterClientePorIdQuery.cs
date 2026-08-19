using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Clientes.Queries
{
    public sealed record ObterClientePorIdQuery(Guid Id) : IQuery<ClienteDetalheDto?>;

    public sealed record ClienteDetalheDto(
        Guid Id,
        string Nome,
        string? Documento,
        string Telefone,
        string? WhatsApp,
        string Email,
        List<TipoCliente> Tipos,
        Guid CorretorResponsavelId,
        OrigemCliente Origem,
        string? CampanhaEspecifica,
        string? Observacoes,
        DateTime CriadoEm,
        DateTime? UltimoContato,
        DateTime? ProximoContato,
        List<PerfilInteresseDto> PerfisInteresse);

    public sealed record PerfilInteresseDto(
        Guid Id,
        TipoNegociacaoImovel TipoNegociacao,
        TipoImovel TipoImovel,
        string? LocalizacaoDesejada,
        decimal ValorMinimo,
        decimal ValorMaximo,
        int? NumeroQuartos,
        FormaPagamento FormaPagamento,
        string? Observacoes);
}

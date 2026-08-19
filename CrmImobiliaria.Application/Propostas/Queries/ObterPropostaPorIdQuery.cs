using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Propostas.Queries
{
    public sealed record ObterPropostaPorIdQuery(Guid Id) : IQuery<PropostaDetalheDto?>;

    public sealed record PropostaDetalheDto(
        Guid Id,
        Guid ClienteId,
        string ClienteNome,
        Guid? ImovelId,
        Guid? AnuncioImovelId,
        string? ImovelEndereco,
        Guid CorretorId,
        string CorretorNome,
        decimal ValorAnunciado,
        decimal ValorProposto,
        decimal? Entrada,
        int? NumeroParcelas,
        decimal? ValorParcela,
        FormaPagamento FormaPagamento,
        StatusProposta Status,
        string? MotivoRecusa,
        List<HistoricoNegociacaoDto> Historico);

    public sealed record HistoricoNegociacaoDto(decimal Valor, OrigemNegociacao Origem, DateTime DataHora, string? Observacao);
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Comissoes.Queries
{
    public sealed record ObterComissaoPorIdQuery(Guid Id) : IQuery<ComissaoDetalheDto?>;

    public sealed record ComissaoDetalheDto(
        Guid Id,
        OrigemComissao OrigemTipo,
        Guid OrigemId,
        string? ClienteNome,
        string? ImovelEndereco,
        decimal ValorBase,
        decimal PercentualComissao,
        decimal ComissaoTotal,
        decimal ComissaoRecebida,
        decimal ComissaoDistribuida,
        decimal Saldo,
        List<RateioComissaoItemDto> Rateio);

    public sealed record RateioComissaoItemDto(PapelComissao Papel, decimal Percentual, decimal Valor, Guid? BeneficiarioId);
}

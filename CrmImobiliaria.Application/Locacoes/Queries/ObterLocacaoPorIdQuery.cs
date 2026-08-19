using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Locacoes.Queries
{
    public sealed record ObterLocacaoPorIdQuery(Guid Id) : IQuery<LocacaoDetalheDto?>;

    public sealed record LocacaoDetalheDto(
        Guid Id,
        string ProprietarioNome,
        string LocatarioNome,
        string ImovelEndereco,
        string CorretorNome,
        EstagioLocacao EstagioAtual,
        StatusLocacao Status,
        decimal? ValorAluguel,
        DateOnly? DataInicial,
        DateOnly? DataFinal,
        int? DiaVencimento,
        decimal? Garantia,
        decimal? TaxaAdministracao,
        IndiceReajuste? IndiceReajuste,
        string? MotivoReprovacaoOuCancelamento);
}

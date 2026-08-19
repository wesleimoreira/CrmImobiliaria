using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Queries
{
    public sealed record ObterSolicitacaoManutencaoPorIdQuery(Guid Id) : IQuery<SolicitacaoManutencaoDetalheDto?>;

    public sealed record OrcamentoItemDto(Guid Id, string PrestadorNome, decimal Valor, string? Descricao, StatusOrcamento Status);

    public sealed record SolicitacaoManutencaoDetalheDto(
        Guid Id, string ImovelEndereco, string SolicitanteNome, string Descricao, StatusSolicitacaoManutencao Status,
        Guid? OrcamentoAprovadoId, DateTime? DataConclusao, List<OrcamentoItemDto> Orcamentos);
}

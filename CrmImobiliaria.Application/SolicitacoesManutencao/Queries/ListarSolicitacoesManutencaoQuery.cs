using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Queries
{
    public sealed record ListarSolicitacoesManutencaoQuery : IQuery<List<SolicitacaoManutencaoListaItemDto>>;

    public sealed record SolicitacaoManutencaoListaItemDto(
        Guid Id, string ImovelEndereco, string SolicitanteNome, string Descricao, StatusSolicitacaoManutencao Status);
}

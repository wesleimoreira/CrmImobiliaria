using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record AbrirSolicitacaoManutencaoCommand(Guid ImovelId, Guid SolicitanteId, string? Descricao, Guid? LocacaoId) : ICommand<Guid>;
}

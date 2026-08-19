using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record AprovarOrcamentoCommand(Guid SolicitacaoId, Guid OrcamentoId) : ICommand<bool>;
}

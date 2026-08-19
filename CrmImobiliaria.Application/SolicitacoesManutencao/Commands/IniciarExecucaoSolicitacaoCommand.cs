using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record IniciarExecucaoSolicitacaoCommand(Guid Id) : ICommand<bool>;
}

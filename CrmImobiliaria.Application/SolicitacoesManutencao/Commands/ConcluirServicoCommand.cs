using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record ConcluirServicoCommand(Guid Id, DateTime DataConclusao) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record CancelarSolicitacaoCommand(Guid Id, string? Motivo) : ICommand<bool>;
}

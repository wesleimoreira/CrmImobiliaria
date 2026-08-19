using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed record AdicionarOrcamentoCommand(Guid SolicitacaoId, Guid PrestadorId, decimal Valor, string? Descricao) : ICommand<Guid>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed class AprovarOrcamentoCommandHandler(ISolicitacaoManutencaoRepository solicitacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AprovarOrcamentoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AprovarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            var solicitacao = await solicitacaoRepository.ObterPorIdAsync(command.SolicitacaoId, cancellationToken);
            if (solicitacao is null)
                return Result<bool>.Failure("Solicitação não encontrada.");

            var resultado = solicitacao.AprovarOrcamento(command.OrcamentoId);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

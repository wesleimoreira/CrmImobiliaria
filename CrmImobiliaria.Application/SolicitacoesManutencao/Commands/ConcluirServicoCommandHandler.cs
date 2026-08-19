using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed class ConcluirServicoCommandHandler(ISolicitacaoManutencaoRepository solicitacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<ConcluirServicoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(ConcluirServicoCommand command, CancellationToken cancellationToken = default)
        {
            var solicitacao = await solicitacaoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (solicitacao is null)
                return Result<bool>.Failure("Solicitação não encontrada.");

            var resultado = solicitacao.ConcluirServico(command.DataConclusao);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

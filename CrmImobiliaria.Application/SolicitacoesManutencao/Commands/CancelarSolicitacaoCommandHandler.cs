using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed class CancelarSolicitacaoCommandHandler(ISolicitacaoManutencaoRepository solicitacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CancelarSolicitacaoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(CancelarSolicitacaoCommand command, CancellationToken cancellationToken = default)
        {
            var solicitacao = await solicitacaoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (solicitacao is null)
                return Result<bool>.Failure("Solicitação não encontrada.");

            var resultado = solicitacao.Cancelar(command.Motivo ?? string.Empty);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed class AbrirSolicitacaoManutencaoCommandHandler(ISolicitacaoManutencaoRepository solicitacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AbrirSolicitacaoManutencaoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AbrirSolicitacaoManutencaoCommand command, CancellationToken cancellationToken = default)
        {
            var solicitacaoResultado = SolicitacaoManutencao.Abrir(command.ImovelId, command.SolicitanteId, command.Descricao, command.LocacaoId);
            if (!solicitacaoResultado.IsSuccess)
                return Result<Guid>.Failure(solicitacaoResultado.Error!);

            var solicitacao = solicitacaoResultado.Value!;
            solicitacaoRepository.Adicionar(solicitacao);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(solicitacao.Id);
        }
    }
}

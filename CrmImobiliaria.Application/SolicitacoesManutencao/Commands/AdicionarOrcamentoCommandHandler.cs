using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.SolicitacoesManutencao.Commands
{
    public sealed class AdicionarOrcamentoCommandHandler(ISolicitacaoManutencaoRepository solicitacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AdicionarOrcamentoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AdicionarOrcamentoCommand command, CancellationToken cancellationToken = default)
        {
            var solicitacao = await solicitacaoRepository.ObterPorIdAsync(command.SolicitacaoId, cancellationToken);
            if (solicitacao is null)
                return Result<Guid>.Failure("Solicitação não encontrada.");

            var valorResultado = Dinheiro.Criar(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<Guid>.Failure(valorResultado.Error!);

            var orcamentoResultado = solicitacao.AdicionarOrcamento(command.PrestadorId, valorResultado.Value!, command.Descricao);
            if (!orcamentoResultado.IsSuccess)
                return Result<Guid>.Failure(orcamentoResultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<Guid>.Success(orcamentoResultado.Value!.Id);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed class AtualizarSituacaoDocumentalCommandHandler(IVendaRepository vendaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AtualizarSituacaoDocumentalCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AtualizarSituacaoDocumentalCommand command, CancellationToken cancellationToken = default)
        {
            var venda = await vendaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (venda is null)
                return Result<bool>.Failure("Venda não encontrada.");

            venda.AtualizarSituacaoDocumental(command.Situacao);
            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

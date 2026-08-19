using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed class CancelarPropostaCommandHandler(IPropostaRepository propostaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CancelarPropostaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(CancelarPropostaCommand command, CancellationToken cancellationToken = default)
        {
            var proposta = await propostaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (proposta is null)
                return Result<bool>.Failure("Proposta não encontrada.");

            var resultado = proposta.Cancelar();
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

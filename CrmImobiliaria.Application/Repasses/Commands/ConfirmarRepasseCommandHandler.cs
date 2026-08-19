using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed class ConfirmarRepasseCommandHandler(IRepasseRepository repasseRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<ConfirmarRepasseCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(ConfirmarRepasseCommand command, CancellationToken cancellationToken = default)
        {
            var repasse = await repasseRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (repasse is null)
                return Result<bool>.Failure("Repasse não encontrado.");

            var resultado = repasse.Confirmar(command.DataRepasse, command.Comprovante);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

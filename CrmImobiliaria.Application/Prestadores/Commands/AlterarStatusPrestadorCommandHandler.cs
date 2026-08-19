using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Prestadores.Commands
{
    public sealed class AlterarStatusPrestadorCommandHandler(IPrestadorRepository prestadorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AlterarStatusPrestadorCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AlterarStatusPrestadorCommand command, CancellationToken cancellationToken = default)
        {
            var prestador = await prestadorRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (prestador is null)
                return Result<bool>.Failure("Prestador não encontrado.");

            if (command.Ativo)
                prestador.Reativar();
            else
                prestador.Inativar();

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

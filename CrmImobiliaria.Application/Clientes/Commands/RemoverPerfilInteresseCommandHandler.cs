using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed class RemoverPerfilInteresseCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<RemoverPerfilInteresseCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(RemoverPerfilInteresseCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.ClienteId, cancellationToken);
            if (cliente is null)
                return Result<bool>.Failure("Cliente não encontrado.");

            cliente.RemoverPerfilInteresse(command.PerfilId);
            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

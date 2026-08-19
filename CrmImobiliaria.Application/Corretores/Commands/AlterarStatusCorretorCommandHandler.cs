using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed class AlterarStatusCorretorCommandHandler(ICorretorRepository corretorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AlterarStatusCorretorCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AlterarStatusCorretorCommand command, CancellationToken cancellationToken = default)
        {
            var corretor = await corretorRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (corretor is null)
                return Result<bool>.Failure("Corretor não encontrado.");

            switch (command.NovoStatus)
            {
                case StatusCorretor.Ativo:
                    corretor.Reativar();
                    break;
                case StatusCorretor.Inativo:
                    corretor.Inativar();
                    break;
                case StatusCorretor.Suspenso:
                    corretor.Suspender();
                    break;
            }

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

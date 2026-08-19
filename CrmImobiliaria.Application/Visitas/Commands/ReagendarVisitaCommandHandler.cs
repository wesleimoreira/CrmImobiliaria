using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public sealed class ReagendarVisitaCommandHandler(IVisitaRepository visitaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<ReagendarVisitaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(ReagendarVisitaCommand command, CancellationToken cancellationToken = default)
        {
            var visita = await visitaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (visita is null)
                return Result<bool>.Failure("Visita não encontrada.");

            var resultado = visita.Reagendar(command.NovaDataHora);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

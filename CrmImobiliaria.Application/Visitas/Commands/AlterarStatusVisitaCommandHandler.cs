using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public sealed class AlterarStatusVisitaCommandHandler(IVisitaRepository visitaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AlterarStatusVisitaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AlterarStatusVisitaCommand command, CancellationToken cancellationToken = default)
        {
            var visita = await visitaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (visita is null)
                return Result<bool>.Failure("Visita não encontrada.");

            var resultado = command.Acao switch
            {
                AcaoVisita.Confirmar => visita.Confirmar(),
                AcaoVisita.RegistrarRealizada => visita.RegistrarRealizada(command.Texto),
                AcaoVisita.MarcarNaoCompareceu => visita.MarcarNaoCompareceu(command.Texto),
                AcaoVisita.Cancelar => visita.Cancelar(command.Texto),
                _ => Result<bool>.Failure("Ação inválida.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

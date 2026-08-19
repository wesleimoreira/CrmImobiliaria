using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed class CancelarReservaLoteCommandHandler(
        IReservaLoteRepository reservaLoteRepository, ILoteRepository loteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CancelarReservaLoteCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(CancelarReservaLoteCommand command, CancellationToken cancellationToken = default)
        {
            var reserva = await reservaLoteRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (reserva is null)
                return Result<bool>.Failure("Reserva não encontrada.");

            var resultado = reserva.Cancelar();
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            var lote = await loteRepository.ObterPorIdAsync(reserva.LoteId, cancellationToken);
            if (lote is { Status: StatusLote.Reservado or StatusLote.EmProposta })
                lote.LiberarReserva();

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

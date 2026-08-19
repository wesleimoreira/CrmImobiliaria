using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed class ConverterReservaLoteCommandHandler(
        IReservaLoteRepository reservaLoteRepository, ILoteRepository loteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<ConverterReservaLoteCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(ConverterReservaLoteCommand command, CancellationToken cancellationToken = default)
        {
            var reserva = await reservaLoteRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (reserva is null)
                return Result<bool>.Failure("Reserva não encontrada.");

            var resultado = reserva.Converter();
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            var lote = await loteRepository.ObterPorIdAsync(reserva.LoteId, cancellationToken);
            if (lote is { Status: StatusLote.Reservado })
            {
                var loteResultado = lote.IniciarProposta();
                if (!loteResultado.IsSuccess)
                    return Result<bool>.Failure(loteResultado.Error!);
            }

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

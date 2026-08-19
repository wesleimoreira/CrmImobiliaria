using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.ReservasLote.Commands
{
    public sealed class CriarReservaLoteCommandHandler(
        IReservaLoteRepository reservaLoteRepository, ILoteRepository loteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarReservaLoteCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarReservaLoteCommand command, CancellationToken cancellationToken = default)
        {
            var lote = await loteRepository.ObterPorIdAsync(command.LoteId, cancellationToken);
            if (lote is null)
                return Result<Guid>.Failure("Lote não encontrado.");

            var reservaResultado = ReservaLote.Criar(command.LoteId, command.ClienteId, command.CorretorId, command.DataReserva, command.PrazoDias);
            if (!reservaResultado.IsSuccess)
                return Result<Guid>.Failure(reservaResultado.Error!);

            var loteReservarResultado = lote.Reservar();
            if (!loteReservarResultado.IsSuccess)
                return Result<Guid>.Failure(loteReservarResultado.Error!);

            var reserva = reservaResultado.Value!;
            reservaLoteRepository.Adicionar(reserva);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(reserva.Id);
        }
    }
}

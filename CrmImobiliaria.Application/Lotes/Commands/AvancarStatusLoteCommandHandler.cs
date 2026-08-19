using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Lotes.Commands
{
    public sealed class AvancarStatusLoteCommandHandler(ILoteRepository loteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AvancarStatusLoteCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AvancarStatusLoteCommand command, CancellationToken cancellationToken = default)
        {
            var lote = await loteRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (lote is null)
                return Result<bool>.Failure("Lote não encontrado.");

            var resultado = command.Acao switch
            {
                AcaoLote.Reservar => lote.Reservar(),
                AcaoLote.IniciarProposta => lote.IniciarProposta(),
                AcaoLote.Vender => lote.Vender(),
                AcaoLote.LiberarReserva => lote.LiberarReserva(),
                AcaoLote.Distratar => lote.Distratar(command.Motivo ?? string.Empty),
                AcaoLote.Bloquear => lote.Bloquear(),
                AcaoLote.Desbloquear => lote.Desbloquear(),
                _ => Result<bool>.Failure("Ação inválida.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

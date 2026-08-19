using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed class CancelarVistoriaCommandHandler(IVistoriaRepository vistoriaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CancelarVistoriaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(CancelarVistoriaCommand command, CancellationToken cancellationToken = default)
        {
            var vistoria = await vistoriaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (vistoria is null)
                return Result<bool>.Failure("Vistoria não encontrada.");

            var resultado = vistoria.Cancelar(command.Motivo);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

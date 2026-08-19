using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed class RegistrarRealizacaoVistoriaCommandHandler(IVistoriaRepository vistoriaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<RegistrarRealizacaoVistoriaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(RegistrarRealizacaoVistoriaCommand command, CancellationToken cancellationToken = default)
        {
            var vistoria = await vistoriaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (vistoria is null)
                return Result<bool>.Failure("Vistoria não encontrada.");

            var resultado = vistoria.RegistrarRealizacao(command.DataRealizada, command.Observacoes);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

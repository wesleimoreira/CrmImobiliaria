using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed class AgendarVistoriaCommandHandler(IVistoriaRepository vistoriaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AgendarVistoriaCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AgendarVistoriaCommand command, CancellationToken cancellationToken = default)
        {
            var vistoriaResultado = Vistoria.Agendar(command.LocacaoId, command.ImovelId, command.Tipo, command.DataAgendada, command.ResponsavelId);
            if (!vistoriaResultado.IsSuccess)
                return Result<Guid>.Failure(vistoriaResultado.Error!);

            var vistoria = vistoriaResultado.Value!;
            vistoriaRepository.Adicionar(vistoria);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(vistoria.Id);
        }
    }
}

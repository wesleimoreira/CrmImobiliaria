using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed class ReabrirLeadCommandHandler(ILeadRepository leadRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<ReabrirLeadCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(ReabrirLeadCommand command, CancellationToken cancellationToken = default)
        {
            var lead = await leadRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (lead is null)
                return Result<bool>.Failure("Lead não encontrado.");

            var resultado = lead.Reabrir();
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

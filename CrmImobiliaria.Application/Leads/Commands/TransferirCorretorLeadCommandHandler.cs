using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Leads.Commands
{
    public sealed class TransferirCorretorLeadCommandHandler(
        ILeadRepository leadRepository, ICorretorRepository corretorRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<TransferirCorretorLeadCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(TransferirCorretorLeadCommand command, CancellationToken cancellationToken = default)
        {
            var lead = await leadRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (lead is null)
                return Result<bool>.Failure("Lead não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.NovoCorretorId, cancellationToken);
            if (corretor is null)
                return Result<bool>.Failure("Corretor não encontrado.");

            var resultado = lead.TransferirCorretor(command.NovoCorretorId);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

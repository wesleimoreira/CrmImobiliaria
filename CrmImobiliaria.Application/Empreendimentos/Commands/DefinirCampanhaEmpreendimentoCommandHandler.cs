using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public sealed class DefinirCampanhaEmpreendimentoCommandHandler(IEmpreendimentoRepository empreendimentoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<DefinirCampanhaEmpreendimentoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(DefinirCampanhaEmpreendimentoCommand command, CancellationToken cancellationToken = default)
        {
            var empreendimento = await empreendimentoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (empreendimento is null)
                return Result<bool>.Failure("Empreendimento não encontrado.");

            empreendimento.DefinirCampanha(command.Campanha);
            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

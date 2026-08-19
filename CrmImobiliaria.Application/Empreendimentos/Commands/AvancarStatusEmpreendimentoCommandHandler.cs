using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public sealed class AvancarStatusEmpreendimentoCommandHandler(IEmpreendimentoRepository empreendimentoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AvancarStatusEmpreendimentoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AvancarStatusEmpreendimentoCommand command, CancellationToken cancellationToken = default)
        {
            var empreendimento = await empreendimentoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (empreendimento is null)
                return Result<bool>.Failure("Empreendimento não encontrado.");

            var resultado = command.Acao switch
            {
                AcaoEmpreendimento.Lancar => empreendimento.Lancar(command.DataLancamento ?? DateOnly.FromDateTime(DateTime.Today)),
                AcaoEmpreendimento.IniciarComercializacao => empreendimento.IniciarComercializacao(),
                AcaoEmpreendimento.Suspender => empreendimento.Suspender(),
                AcaoEmpreendimento.Encerrar => empreendimento.Encerrar(),
                _ => Result<bool>.Failure("Ação inválida.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

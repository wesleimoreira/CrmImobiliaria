using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed class AdicionarDespesaRepasseCommandHandler(IRepasseRepository repasseRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AdicionarDespesaRepasseCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AdicionarDespesaRepasseCommand command, CancellationToken cancellationToken = default)
        {
            var repasse = await repasseRepository.ObterPorIdAsync(command.RepasseId, cancellationToken);
            if (repasse is null)
                return Result<bool>.Failure("Repasse não encontrado.");

            var valorResultado = Dinheiro.CriarPositivo(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<bool>.Failure(valorResultado.Error!);

            var resultado = repasse.AdicionarDespesa(command.Descricao, valorResultado.Value!);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

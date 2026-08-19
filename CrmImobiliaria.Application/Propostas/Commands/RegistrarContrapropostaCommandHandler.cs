using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed class RegistrarContrapropostaCommandHandler(IPropostaRepository propostaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<RegistrarContrapropostaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(RegistrarContrapropostaCommand command, CancellationToken cancellationToken = default)
        {
            var proposta = await propostaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (proposta is null)
                return Result<bool>.Failure("Proposta não encontrada.");

            var valorResultado = Dinheiro.CriarPositivo(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<bool>.Failure(valorResultado.Error!);

            var resultado = proposta.RegistrarContraproposta(valorResultado.Value!, command.Origem, command.Observacao);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

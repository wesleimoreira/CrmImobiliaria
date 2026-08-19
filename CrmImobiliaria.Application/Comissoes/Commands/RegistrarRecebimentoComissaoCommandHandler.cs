using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed class RegistrarRecebimentoComissaoCommandHandler(IComissaoRepository comissaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<RegistrarRecebimentoComissaoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(RegistrarRecebimentoComissaoCommand command, CancellationToken cancellationToken = default)
        {
            var comissao = await comissaoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (comissao is null)
                return Result<bool>.Failure("Comissão não encontrada.");

            var valorResultado = Dinheiro.CriarPositivo(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<bool>.Failure(valorResultado.Error!);

            var resultado = comissao.RegistrarRecebimento(valorResultado.Value!);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

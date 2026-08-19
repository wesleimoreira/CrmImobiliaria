using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed class AnexarContratoVendaCommandHandler(IVendaRepository vendaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AnexarContratoVendaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AnexarContratoVendaCommand command, CancellationToken cancellationToken = default)
        {
            var venda = await vendaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (venda is null)
                return Result<bool>.Failure("Venda não encontrada.");

            var resultado = venda.AnexarContrato(command.NumeroContrato, command.UrlContrato);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

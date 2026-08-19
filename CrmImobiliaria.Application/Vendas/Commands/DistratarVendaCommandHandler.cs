using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed class DistratarVendaCommandHandler(IVendaRepository vendaRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<DistratarVendaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(DistratarVendaCommand command, CancellationToken cancellationToken = default)
        {
            var venda = await vendaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (venda is null)
                return Result<bool>.Failure("Venda não encontrada.");

            var resultado = venda.Distratar(command.Motivo);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

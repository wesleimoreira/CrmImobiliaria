using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.PagamentosAluguel.Commands
{
    public sealed class RegistrarPagamentoAluguelCommandHandler(IPagamentoAluguelRepository pagamentoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<RegistrarPagamentoAluguelCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(RegistrarPagamentoAluguelCommand command, CancellationToken cancellationToken = default)
        {
            var pagamento = await pagamentoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (pagamento is null)
                return Result<bool>.Failure("Pagamento não encontrado.");

            var valorResultado = Dinheiro.CriarPositivo(command.ValorPago);
            if (!valorResultado.IsSuccess)
                return Result<bool>.Failure(valorResultado.Error!);

            var resultado = pagamento.RegistrarPagamento(command.DataPagamento, valorResultado.Value!);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.PagamentosAluguel.Commands
{
    public sealed class GerarPagamentoAluguelCommandHandler(
        IPagamentoAluguelRepository pagamentoRepository, ILocacaoRepository locacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<GerarPagamentoAluguelCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(GerarPagamentoAluguelCommand command, CancellationToken cancellationToken = default)
        {
            var locacao = await locacaoRepository.ObterPorIdAsync(command.LocacaoId, cancellationToken);
            if (locacao is null)
                return Result<Guid>.Failure("Locação não encontrada.");

            var competenciaResultado = Competencia.Criar(command.Mes, command.Ano);
            if (!competenciaResultado.IsSuccess)
                return Result<Guid>.Failure(competenciaResultado.Error!);

            var valorResultado = Dinheiro.CriarPositivo(command.ValorDevido);
            if (!valorResultado.IsSuccess)
                return Result<Guid>.Failure(valorResultado.Error!);

            var pagamentoResultado = PagamentoAluguel.Gerar(command.LocacaoId, competenciaResultado.Value!, valorResultado.Value!, command.DataVencimento);
            if (!pagamentoResultado.IsSuccess)
                return Result<Guid>.Failure(pagamentoResultado.Error!);

            var pagamento = pagamentoResultado.Value!;
            pagamentoRepository.Adicionar(pagamento);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(pagamento.Id);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed class GerarRepasseCommandHandler(
        IRepasseRepository repasseRepository,
        IPagamentoAluguelRepository pagamentoAluguelRepository,
        ILocacaoRepository locacaoRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<GerarRepasseCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(GerarRepasseCommand command, CancellationToken cancellationToken = default)
        {
            var pagamento = await pagamentoAluguelRepository.ObterPorIdAsync(command.PagamentoAluguelId, cancellationToken);
            if (pagamento is null)
                return Result<Guid>.Failure("Pagamento de aluguel não encontrado.");

            if (pagamento.Status != StatusPagamentoAluguel.Pago)
                return Result<Guid>.Failure("Só é possível gerar repasse para um pagamento já registrado.");

            var locacao = await locacaoRepository.ObterPorIdAsync(pagamento.LocacaoId, cancellationToken);
            if (locacao is null)
                return Result<Guid>.Failure("Locação não encontrada.");

            if (locacao.TaxaAdministracao is null)
                return Result<Guid>.Failure("Locação não possui taxa de administração definida.");

            if (await repasseRepository.ExisteParaPagamentoAsync(pagamento.Id, cancellationToken))
                return Result<Guid>.Failure("Já existe um repasse gerado para este pagamento.");

            var repasseResultado = Repasse.Calcular(
                locacao.Id, pagamento.Id, pagamento.Competencia, pagamento.ValorPago!, locacao.TaxaAdministracao);
            if (!repasseResultado.IsSuccess)
                return Result<Guid>.Failure(repasseResultado.Error!);

            var repasse = repasseResultado.Value!;
            repasseRepository.Adicionar(repasse);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(repasse.Id);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public sealed class FormalizarContratoLocacaoCommandHandler(ILocacaoRepository locacaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<FormalizarContratoLocacaoCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(FormalizarContratoLocacaoCommand command, CancellationToken cancellationToken = default)
        {
            var locacao = await locacaoRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (locacao is null)
                return Result<bool>.Failure("Locação não encontrada.");

            var valorAluguelResultado = Dinheiro.CriarPositivo(command.ValorAluguel);
            if (!valorAluguelResultado.IsSuccess)
                return Result<bool>.Failure(valorAluguelResultado.Error!);

            var periodoResultado = PeriodoContrato.Criar(command.DataInicial, command.DataFinal);
            if (!periodoResultado.IsSuccess)
                return Result<bool>.Failure(periodoResultado.Error!);

            var garantiaResultado = Dinheiro.CriarPositivo(command.Garantia);
            if (!garantiaResultado.IsSuccess)
                return Result<bool>.Failure(garantiaResultado.Error!);

            var taxaResultado = Percentual.Criar(command.TaxaAdministracao);
            if (!taxaResultado.IsSuccess)
                return Result<bool>.Failure(taxaResultado.Error!);

            var resultado = locacao.FormalizarContrato(
                valorAluguelResultado.Value!, periodoResultado.Value!, command.DiaVencimento,
                garantiaResultado.Value!, taxaResultado.Value!, command.IndiceReajuste);

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

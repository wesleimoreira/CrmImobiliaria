using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed class GerarComissaoCommandHandler(
        IComissaoRepository comissaoRepository,
        IVendaRepository vendaRepository,
        IRegraComissaoRepository regraComissaoRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<GerarComissaoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(GerarComissaoCommand command, CancellationToken cancellationToken = default)
        {
            var venda = await vendaRepository.ObterPorIdAsync(command.VendaId, cancellationToken);
            if (venda is null)
                return Result<Guid>.Failure("Venda não encontrada.");

            var regra = await regraComissaoRepository.ObterPorIdAsync(command.RegraComissaoId, cancellationToken);
            if (regra is null)
                return Result<Guid>.Failure("Regra de comissão não encontrada.");

            if (await comissaoRepository.ExisteParaOrigemAsync(OrigemComissao.Venda, venda.Id, cancellationToken))
                return Result<Guid>.Failure("Já existe uma comissão gerada para esta venda.");

            var comissaoResultado = Comissao.Gerar(OrigemComissao.Venda, venda.Id, regra, venda.ValorFinal);
            if (!comissaoResultado.IsSuccess)
                return Result<Guid>.Failure(comissaoResultado.Error!);

            var comissao = comissaoResultado.Value!;
            comissaoRepository.Adicionar(comissao);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(comissao.Id);
        }
    }
}

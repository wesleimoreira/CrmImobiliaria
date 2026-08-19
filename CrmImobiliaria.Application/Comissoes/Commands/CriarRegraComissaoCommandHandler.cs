using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed class CriarRegraComissaoCommandHandler(IRegraComissaoRepository regraComissaoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarRegraComissaoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarRegraComissaoCommand command, CancellationToken cancellationToken = default)
        {
            var percentualTotalResultado = Percentual.Criar(command.PercentualComissaoTotal);
            if (!percentualTotalResultado.IsSuccess)
                return Result<Guid>.Failure(percentualTotalResultado.Error!);

            var regraResultado = RegraComissao.Criar(command.Nome, percentualTotalResultado.Value!, imovelId: command.ImovelId);
            if (!regraResultado.IsSuccess)
                return Result<Guid>.Failure(regraResultado.Error!);

            var regra = regraResultado.Value!;

            var itens = new List<ItemRateio>();
            foreach (var (papel, valor) in command.Rateio)
            {
                if (valor <= 0) continue;

                var percentualResultado = Percentual.Criar(valor);
                if (!percentualResultado.IsSuccess)
                    return Result<Guid>.Failure(percentualResultado.Error!);

                itens.Add(new ItemRateio(papel, percentualResultado.Value!));
            }

            var rateioResultado = regra.DefinirRateio(itens);
            if (!rateioResultado.IsSuccess)
                return Result<Guid>.Failure(rateioResultado.Error!);

            regraComissaoRepository.Adicionar(regra);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(regra.Id);
        }
    }
}

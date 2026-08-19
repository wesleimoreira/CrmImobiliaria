using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Lotes.Commands
{
    public sealed class CriarLoteCommandHandler(
        ILoteRepository loteRepository, IEmpreendimentoRepository empreendimentoRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<CriarLoteCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarLoteCommand command, CancellationToken cancellationToken = default)
        {
            var empreendimento = await empreendimentoRepository.ObterPorIdAsync(command.EmpreendimentoId, cancellationToken);
            if (empreendimento is null)
                return Result<Guid>.Failure("Empreendimento não encontrado.");

            var areaResultado = Area.Criar(command.AreaM2);
            if (!areaResultado.IsSuccess)
                return Result<Guid>.Failure(areaResultado.Error!);

            var valorResultado = Dinheiro.CriarPositivo(command.Valor);
            if (!valorResultado.IsSuccess)
                return Result<Guid>.Failure(valorResultado.Error!);

            var entradaResultado = Dinheiro.CriarPositivo(command.EntradaMinima);
            if (!entradaResultado.IsSuccess)
                return Result<Guid>.Failure(entradaResultado.Error!);

            Dinheiro? valorPromocional = null;
            if (command.ValorPromocional is not null)
            {
                var promocionalResultado = Dinheiro.CriarPositivo(command.ValorPromocional.Value);
                if (!promocionalResultado.IsSuccess)
                    return Result<Guid>.Failure(promocionalResultado.Error!);

                valorPromocional = promocionalResultado.Value!;
            }

            var loteResultado = Lote.Criar(
                command.EmpreendimentoId, command.Quadra, command.Numero, areaResultado.Value!,
                valorResultado.Value!, entradaResultado.Value!, command.ParcelamentoMaximo, valorPromocional);

            if (!loteResultado.IsSuccess)
                return Result<Guid>.Failure(loteResultado.Error!);

            var lote = loteResultado.Value!;
            loteRepository.Adicionar(lote);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(lote.Id);
        }
    }
}

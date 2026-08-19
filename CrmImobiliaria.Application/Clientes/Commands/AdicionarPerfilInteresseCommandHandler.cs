using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed class AdicionarPerfilInteresseCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AdicionarPerfilInteresseCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AdicionarPerfilInteresseCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.ClienteId, cancellationToken);
            if (cliente is null)
                return Result<bool>.Failure("Cliente não encontrado.");

            var minimoResultado = Dinheiro.Criar(command.ValorMinimo);
            if (!minimoResultado.IsSuccess)
                return Result<bool>.Failure(minimoResultado.Error!);

            var maximoResultado = Dinheiro.Criar(command.ValorMaximo);
            if (!maximoResultado.IsSuccess)
                return Result<bool>.Failure(maximoResultado.Error!);

            var faixaResultado = FaixaDeValor.Criar(minimoResultado.Value!, maximoResultado.Value!);
            if (!faixaResultado.IsSuccess)
                return Result<bool>.Failure(faixaResultado.Error!);

            var perfilResultado = PerfilInteresse.Criar(
                command.TipoNegociacao, command.TipoImovel, faixaResultado.Value!,
                command.LocalizacaoDesejada, command.NumeroQuartos, command.FormaPagamento, command.Observacoes);

            if (!perfilResultado.IsSuccess)
                return Result<bool>.Failure(perfilResultado.Error!);

            var resultado = cliente.AdicionarPerfilInteresse(perfilResultado.Value!);
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

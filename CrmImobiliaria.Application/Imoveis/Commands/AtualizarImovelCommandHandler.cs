using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed class AtualizarImovelCommandHandler(
        IImovelRepository imovelRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<AtualizarImovelCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AtualizarImovelCommand command, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (imovel is null)
                return Result<bool>.Failure("Imóvel não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorCaptadorId, cancellationToken);
            if (corretor is null)
                return Result<bool>.Failure("Corretor captador não encontrado.");

            if (command.Suites > command.Quartos)
                return Result<bool>.Failure("Número de suítes não pode ser maior que o de quartos.");

            var enderecoResultado = Endereco.Criar(command.Logradouro, command.Numero, command.Complemento, command.Bairro, command.Cidade, command.Uf, command.Cep);
            if (!enderecoResultado.IsSuccess)
                return Result<bool>.Failure(enderecoResultado.Error!);

            imovel.AtualizarEndereco(enderecoResultado.Value!);
            imovel.AtualizarCaracteristicasFisicas(command.Quartos, command.Suites, command.Garagem);
            imovel.DefinirCorretorCaptador(command.CorretorCaptadorId);

            foreach (var caracteristica in command.Caracteristicas.Except(imovel.Caracteristicas, StringComparer.OrdinalIgnoreCase).ToList())
                imovel.AdicionarCaracteristica(caracteristica);

            foreach (var caracteristica in imovel.Caracteristicas.Except(command.Caracteristicas, StringComparer.OrdinalIgnoreCase).ToList())
                imovel.RemoverCaracteristica(caracteristica);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

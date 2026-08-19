using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed class CriarImovelCommandHandler(
        IImovelRepository imovelRepository,
        IClienteRepository clienteRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CriarImovelCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarImovelCommand command, CancellationToken cancellationToken = default)
        {
            var proprietario = await clienteRepository.ObterPorIdAsync(command.ProprietarioId, cancellationToken);
            if (proprietario is null)
                return Result<Guid>.Failure("Proprietário não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorCaptadorId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor captador não encontrado.");

            var enderecoResultado = Endereco.Criar(command.Logradouro, command.Numero, command.Complemento, command.Bairro, command.Cidade, command.Uf, command.Cep);
            if (!enderecoResultado.IsSuccess)
                return Result<Guid>.Failure(enderecoResultado.Error!);

            var areaResultado = Area.Criar(command.AreaM2);
            if (!areaResultado.IsSuccess)
                return Result<Guid>.Failure(areaResultado.Error!);

            var imovelResultado = Imovel.Criar(
                command.ProprietarioId, command.CorretorCaptadorId, command.Tipo,
                enderecoResultado.Value!, areaResultado.Value!, command.Quartos, command.Suites, command.Garagem);

            if (!imovelResultado.IsSuccess)
                return Result<Guid>.Failure(imovelResultado.Error!);

            var imovel = imovelResultado.Value!;
            imovelRepository.Adicionar(imovel);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(imovel.Id);
        }
    }
}

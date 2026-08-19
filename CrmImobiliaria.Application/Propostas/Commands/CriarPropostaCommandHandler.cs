using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.ValueObjects;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed class CriarPropostaCommandHandler(
        IPropostaRepository propostaRepository,
        IClienteRepository clienteRepository,
        IImovelRepository imovelRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CriarPropostaCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CriarPropostaCommand command, CancellationToken cancellationToken = default)
        {
            var cliente = await clienteRepository.ObterPorIdAsync(command.ClienteId, cancellationToken);
            if (cliente is null)
                return Result<Guid>.Failure("Cliente não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor não encontrado.");

            var imovel = await imovelRepository.ObterPorIdAsync(command.ImovelId, cancellationToken);
            if (imovel is null)
                return Result<Guid>.Failure("Imóvel não encontrado.");

            var anuncio = imovel.Anuncios.FirstOrDefault(a => a.Id == command.AnuncioImovelId);
            if (anuncio is null)
                return Result<Guid>.Failure("Anúncio não encontrado para este imóvel.");

            var valorPropostoResultado = Dinheiro.CriarPositivo(command.ValorProposto);
            if (!valorPropostoResultado.IsSuccess)
                return Result<Guid>.Failure(valorPropostoResultado.Error!);

            Dinheiro? entrada = null;
            if (command.Entrada is { } valorEntrada)
            {
                var entradaResultado = Dinheiro.CriarPositivo(valorEntrada);
                if (!entradaResultado.IsSuccess)
                    return Result<Guid>.Failure(entradaResultado.Error!);
                entrada = entradaResultado.Value;
            }

            Dinheiro? valorParcela = null;
            if (command.ValorParcela is { } valorDaParcela)
            {
                var parcelaResultado = Dinheiro.CriarPositivo(valorDaParcela);
                if (!parcelaResultado.IsSuccess)
                    return Result<Guid>.Failure(parcelaResultado.Error!);
                valorParcela = parcelaResultado.Value;
            }

            var propostaResultado = Proposta.ParaImovel(
                command.ClienteId, command.ImovelId, command.AnuncioImovelId, command.CorretorId,
                anuncio.Valor, valorPropostoResultado.Value!, command.FormaPagamento,
                entrada: entrada, numeroParcelas: command.NumeroParcelas, valorParcela: valorParcela);

            if (!propostaResultado.IsSuccess)
                return Result<Guid>.Failure(propostaResultado.Error!);

            var proposta = propostaResultado.Value!;
            propostaRepository.Adicionar(proposta);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(proposta.Id);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public sealed class IniciarLocacaoCommandHandler(
        ILocacaoRepository locacaoRepository,
        IClienteRepository clienteRepository,
        IImovelRepository imovelRepository,
        ICorretorRepository corretorRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<IniciarLocacaoCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(IniciarLocacaoCommand command, CancellationToken cancellationToken = default)
        {
            var proprietario = await clienteRepository.ObterPorIdAsync(command.ProprietarioId, cancellationToken);
            if (proprietario is null)
                return Result<Guid>.Failure("Proprietário não encontrado.");

            var locatario = await clienteRepository.ObterPorIdAsync(command.LocatarioId, cancellationToken);
            if (locatario is null)
                return Result<Guid>.Failure("Locatário não encontrado.");

            var corretor = await corretorRepository.ObterPorIdAsync(command.CorretorId, cancellationToken);
            if (corretor is null)
                return Result<Guid>.Failure("Corretor não encontrado.");

            var imovel = await imovelRepository.ObterPorIdAsync(command.ImovelId, cancellationToken);
            if (imovel is null)
                return Result<Guid>.Failure("Imóvel não encontrado.");

            var anuncio = imovel.Anuncios.FirstOrDefault(a => a.Id == command.AnuncioImovelId);
            if (anuncio is null)
                return Result<Guid>.Failure("Anúncio não encontrado para este imóvel.");

            var locacaoResultado = Locacao.Iniciar(
                command.ProprietarioId, command.LocatarioId, command.ImovelId, command.AnuncioImovelId, command.CorretorId, command.LeadId);

            if (!locacaoResultado.IsSuccess)
                return Result<Guid>.Failure(locacaoResultado.Error!);

            if (anuncio.Status is StatusAnuncio.Disponivel or StatusAnuncio.Reservado)
            {
                var negociacaoResultado = anuncio.IniciarNegociacao();
                if (!negociacaoResultado.IsSuccess)
                    return Result<Guid>.Failure(negociacaoResultado.Error!);
            }

            var locacao = locacaoResultado.Value!;
            locacaoRepository.Adicionar(locacao);
            await unitOfWork.SalvarAsync(cancellationToken);

            return Result<Guid>.Success(locacao.Id);
        }
    }
}

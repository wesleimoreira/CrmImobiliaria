using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Enums;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed class AceitarPropostaCommandHandler(
        IPropostaRepository propostaRepository, IImovelRepository imovelRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AceitarPropostaCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AceitarPropostaCommand command, CancellationToken cancellationToken = default)
        {
            var proposta = await propostaRepository.ObterPorIdAsync(command.Id, cancellationToken);
            if (proposta is null)
                return Result<bool>.Failure("Proposta não encontrada.");

            var resultado = proposta.Aceitar();
            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            if (proposta.EhParaImovel)
            {
                var imovel = await imovelRepository.ObterPorIdAsync(proposta.ImovelId!.Value, cancellationToken);
                var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == proposta.AnuncioImovelId);

                if (anuncio is { Status: StatusAnuncio.Disponivel or StatusAnuncio.Reservado })
                {
                    var negociacaoResultado = anuncio.IniciarNegociacao();
                    if (!negociacaoResultado.IsSuccess)
                        return Result<bool>.Failure(negociacaoResultado.Error!);
                }
            }

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Shared;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed class AlterarStatusAnuncioCommandHandler(IImovelRepository imovelRepository, IUnitOfWork unitOfWork)
        : ICommandHandler<AlterarStatusAnuncioCommand, bool>
    {
        public async Task<Result<bool>> HandleAsync(AlterarStatusAnuncioCommand command, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorIdAsync(command.ImovelId, cancellationToken);
            var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == command.AnuncioId);
            if (anuncio is null)
                return Result<bool>.Failure("Anúncio não encontrado.");

            var resultado = command.Acao switch
            {
                AcaoAnuncio.Disponibilizar => anuncio.Disponibilizar(),
                AcaoAnuncio.Reservar => anuncio.Reservar(),
                AcaoAnuncio.IniciarNegociacao => anuncio.IniciarNegociacao(),
                AcaoAnuncio.CancelarNegociacao => anuncio.CancelarNegociacao(),
                AcaoAnuncio.Suspender => anuncio.Suspender(),
                AcaoAnuncio.Reabrir => anuncio.Reabrir(),
                _ => Result<bool>.Failure("Ação inválida.")
            };

            if (!resultado.IsSuccess)
                return Result<bool>.Failure(resultado.Error!);

            await unitOfWork.SalvarAsync(cancellationToken);
            return Result<bool>.Success(true);
        }
    }
}

using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Application.Events
{
    // Reabre o AnuncioImovel quando uma locação ativa é encerrada. Locacao.Encerrar() só roda a
    // partir de Status.Ativa, então o anúncio garantidamente está Alugado nesse ponto.
    public sealed class LocacaoEncerradaEventHandler(IImovelRepository imovelRepository)
        : IDomainEventHandler<LocacaoEncerradaEvent>
    {
        public async Task HandleAsync(LocacaoEncerradaEvent evento, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorAnuncioIdAsync(evento.AnuncioImovelId, cancellationToken);
            var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == evento.AnuncioImovelId);
            var resultado = anuncio?.Reabrir();

            if (resultado is { IsSuccess: false })
                throw new InvalidOperationException(
                    $"Não foi possível reabrir o anúncio {evento.AnuncioImovelId} após o encerramento da locação {evento.LocacaoId}: {resultado.Error}");
        }
    }
}

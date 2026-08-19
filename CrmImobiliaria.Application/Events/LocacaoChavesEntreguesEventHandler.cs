using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Application.Events
{
    // Fecha o AnuncioImovel (vira Alugado) quando as chaves de uma locação são entregues.
    public sealed class LocacaoChavesEntreguesEventHandler(IImovelRepository imovelRepository)
        : IDomainEventHandler<LocacaoChavesEntreguesEvent>
    {
        public async Task HandleAsync(LocacaoChavesEntreguesEvent evento, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorAnuncioIdAsync(evento.AnuncioImovelId, cancellationToken);
            var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == evento.AnuncioImovelId);
            var resultado = anuncio?.Fechar(evento.OcorridoEm);

            if (resultado is { IsSuccess: false })
                throw new InvalidOperationException(
                    $"Não foi possível fechar o anúncio {evento.AnuncioImovelId} após a entrega de chaves da locação {evento.LocacaoId}: {resultado.Error}");
        }
    }
}

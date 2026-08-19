using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Application.Events
{
    // Libera o AnuncioImovel quando uma locação é cancelada. Locacao.Cancelar() nunca roda com a
    // locação Ativa/Encerrada, então o anúncio está sempre em Reservado/EmNegociacao nesse ponto
    // (nunca Alugado) — por isso usa CancelarNegociacao(), não Reabrir().
    public sealed class LocacaoCanceladaEventHandler(IImovelRepository imovelRepository)
        : IDomainEventHandler<LocacaoCanceladaEvent>
    {
        public async Task HandleAsync(LocacaoCanceladaEvent evento, CancellationToken cancellationToken = default)
        {
            var imovel = await imovelRepository.ObterPorAnuncioIdAsync(evento.AnuncioImovelId, cancellationToken);
            var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == evento.AnuncioImovelId);
            var resultado = anuncio?.CancelarNegociacao();

            if (resultado is { IsSuccess: false })
                throw new InvalidOperationException(
                    $"Não foi possível liberar o anúncio {evento.AnuncioImovelId} após o cancelamento da locação {evento.LocacaoId}: {resultado.Error}");
        }
    }
}

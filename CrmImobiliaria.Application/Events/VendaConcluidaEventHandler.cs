using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Application.Events
{
    // Fecha automaticamente o AnuncioImovel ou o Lote quando a venda correspondente é concluída
    // (regra do documento de negócio: "o imóvel deverá mudar automaticamente para Vendido").
    public sealed class VendaConcluidaEventHandler(IImovelRepository imovelRepository, ILoteRepository loteRepository)
        : IDomainEventHandler<VendaConcluidaEvent>
    {
        public async Task HandleAsync(VendaConcluidaEvent evento, CancellationToken cancellationToken = default)
        {
            if (evento.ImovelId is { } imovelId && evento.AnuncioImovelId is { } anuncioImovelId)
            {
                var imovel = await imovelRepository.ObterPorIdAsync(imovelId, cancellationToken);
                var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == anuncioImovelId);
                var resultado = anuncio?.Fechar(evento.OcorridoEm);

                if (resultado is { IsSuccess: false })
                    throw new InvalidOperationException(
                        $"Não foi possível fechar o anúncio {anuncioImovelId} após a venda {evento.VendaId}: {resultado.Error}");
            }
            else if (evento.LoteId is { } loteId)
            {
                var lote = await loteRepository.ObterPorIdAsync(loteId, cancellationToken);
                var resultado = lote?.Vender();

                if (resultado is { IsSuccess: false })
                    throw new InvalidOperationException(
                        $"Não foi possível marcar o lote {loteId} como vendido após a venda {evento.VendaId}: {resultado.Error}");
            }
        }
    }
}

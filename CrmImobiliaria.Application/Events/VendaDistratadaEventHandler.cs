using CrmImobiliaria.Application.Abstractions.Events;
using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Events;

namespace CrmImobiliaria.Application.Events
{
    // Reabre o AnuncioImovel quando uma venda de imóvel avulso é distratada. Lote não é tocado
    // automaticamente: o documento de regras só pede automação para o fechamento da venda,
    // reverter um lote vendido fica pra uma ação explícita futura, não um efeito colateral silencioso.
    public sealed class VendaDistratadaEventHandler(IImovelRepository imovelRepository) : IDomainEventHandler<VendaDistratadaEvent>
    {
        public async Task HandleAsync(VendaDistratadaEvent evento, CancellationToken cancellationToken = default)
        {
            if (evento.ImovelId is null || evento.AnuncioImovelId is not { } anuncioImovelId)
                return;

            var imovel = await imovelRepository.ObterPorIdAsync(evento.ImovelId.Value, cancellationToken);
            var anuncio = imovel?.Anuncios.FirstOrDefault(a => a.Id == anuncioImovelId);
            var resultado = anuncio?.Reabrir();

            if (resultado is { IsSuccess: false })
                throw new InvalidOperationException(
                    $"Não foi possível reabrir o anúncio {anuncioImovelId} após o distrato da venda {evento.VendaId}: {resultado.Error}");
        }
    }
}

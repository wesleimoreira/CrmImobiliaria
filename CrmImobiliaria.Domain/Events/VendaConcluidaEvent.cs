using CrmImobiliaria.Domain.Common;
using CrmImobiliaria.Domain.ValueObjects;

namespace CrmImobiliaria.Domain.Events
{
    // Levantado quando uma Venda é fechada (imóvel avulso OU lote de loteamento).
    // A Application usa esses IDs para fechar o AnuncioImovel (StatusAnuncio.Vendido)
    // ou o Lote (StatusLote.Vendido) correspondente, e para disparar a geração de comissão.
    public sealed record VendaConcluidaEvent(
        Guid VendaId,
        Guid? ImovelId,
        Guid? AnuncioImovelId,
        Guid? LoteId,
        Guid CorretorId,
        Guid ClienteId,
        Dinheiro ValorFinal,
        DateTime OcorridoEm) : IDomainEvent;
}

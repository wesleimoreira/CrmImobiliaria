using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    // Ações manuais permitidas na UI. "Fechar" (Vendido/Alugado) fica de fora de propósito:
    // só deve acontecer via VendaConcluidaEvent/LocacaoChavesEntreguesEvent (automático),
    // nunca como botão solto — senão dá pra marcar um anúncio como vendido sem venda nenhuma.
    public enum AcaoAnuncio
    {
        Disponibilizar,
        Reservar,
        IniciarNegociacao,
        CancelarNegociacao,
        Suspender,
        Reabrir
    }

    public sealed record AlterarStatusAnuncioCommand(Guid ImovelId, Guid AnuncioId, AcaoAnuncio Acao) : ICommand<bool>;
}

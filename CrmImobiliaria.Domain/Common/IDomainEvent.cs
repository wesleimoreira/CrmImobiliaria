namespace CrmImobiliaria.Domain.Common
{
    // Marca eventos levantados por um AggregateRoot quando seu estado muda de um jeito
    // que outros agregados precisam saber (ex: Venda concluída -> AnuncioImovel deve fechar).
    // A Application layer é quem despacha e trata esses eventos; o Domain só os registra.
    public interface IDomainEvent;
}

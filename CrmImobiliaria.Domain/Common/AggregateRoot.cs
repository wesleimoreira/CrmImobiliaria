namespace CrmImobiliaria.Domain.Common
{
    // Marca entidades que são "porta de entrada" — só elas têm repositório próprio.
    // Entidades filhas (ex: AnuncioImovel dentro de Imovel) só são acessadas através da raiz.
    // São também as únicas que levantam eventos de domínio, já que só uma raiz de agregado
    // pode notificar mudanças de estado que precisam ser refletidas em outros agregados.
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _eventos = [];

        public IReadOnlyList<IDomainEvent> EventosDominio => _eventos;

        protected void RegistrarEvento(IDomainEvent evento) => _eventos.Add(evento);

        public void LimparEventosDominio() => _eventos.Clear();
    }
}

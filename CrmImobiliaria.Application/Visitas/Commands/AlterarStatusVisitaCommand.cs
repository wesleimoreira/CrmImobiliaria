using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Visitas.Commands
{
    public enum AcaoVisita
    {
        Confirmar,
        RegistrarRealizada,
        MarcarNaoCompareceu,
        Cancelar
    }

    public sealed record AlterarStatusVisitaCommand(Guid Id, AcaoVisita Acao, string? Texto) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Empreendimentos.Commands
{
    public sealed record DefinirCampanhaEmpreendimentoCommand(Guid Id, string? Campanha) : ICommand<bool>;
}

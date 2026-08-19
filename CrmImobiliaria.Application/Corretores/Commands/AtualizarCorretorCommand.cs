using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed record AtualizarCorretorCommand(Guid Id, string Telefone, string Email, string? Equipe, Guid? GerenteId) : ICommand<bool>;
}

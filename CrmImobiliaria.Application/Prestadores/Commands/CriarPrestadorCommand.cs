using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Prestadores.Commands
{
    public sealed record CriarPrestadorCommand(string? Nome, string? Telefone, string? Email, string? Especialidade) : ICommand<Guid>;
}

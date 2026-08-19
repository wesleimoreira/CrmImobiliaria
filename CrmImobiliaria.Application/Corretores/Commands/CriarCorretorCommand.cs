using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed record CriarCorretorCommand(
        string Nome,
        string CreciNumero,
        TipoCreci CreciTipo,
        string CreciUf,
        string Telefone,
        string Email,
        string? Equipe) : ICommand<Guid>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Prestadores.Commands
{
    public sealed record AlterarStatusPrestadorCommand(Guid Id, bool Ativo) : ICommand<bool>;
}

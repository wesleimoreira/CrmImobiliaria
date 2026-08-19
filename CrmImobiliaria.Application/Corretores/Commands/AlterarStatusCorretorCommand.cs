using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Corretores.Commands
{
    public sealed record AlterarStatusCorretorCommand(Guid Id, StatusCorretor NovoStatus) : ICommand<bool>;
}

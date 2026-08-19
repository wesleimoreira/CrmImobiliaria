using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed record RegistrarRecebimentoComissaoCommand(Guid Id, decimal Valor) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed record RegistrarDistribuicaoComissaoCommand(Guid Id, decimal Valor) : ICommand<bool>;
}

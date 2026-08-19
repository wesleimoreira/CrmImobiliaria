using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed record GerarRepasseCommand(Guid PagamentoAluguelId) : ICommand<Guid>;
}

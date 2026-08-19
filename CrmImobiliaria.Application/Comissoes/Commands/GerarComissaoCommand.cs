using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed record GerarComissaoCommand(Guid VendaId, Guid RegraComissaoId) : ICommand<Guid>;
}

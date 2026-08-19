using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vendas.Commands
{
    public sealed record AtualizarSituacaoDocumentalCommand(Guid Id, SituacaoDocumental Situacao) : ICommand<bool>;
}

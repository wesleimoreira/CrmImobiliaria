using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Repasses.Commands
{
    public sealed record AdicionarDespesaRepasseCommand(Guid RepasseId, string Descricao, decimal Valor) : ICommand<bool>;
}

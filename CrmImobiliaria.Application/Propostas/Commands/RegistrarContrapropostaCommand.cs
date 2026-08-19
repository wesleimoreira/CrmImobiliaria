using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    public sealed record RegistrarContrapropostaCommand(Guid Id, decimal Valor, OrigemNegociacao Origem, string? Observacao) : ICommand<bool>;
}

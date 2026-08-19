using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.PagamentosAluguel.Commands
{
    public sealed record RegistrarPagamentoAluguelCommand(Guid Id, DateOnly DataPagamento, decimal ValorPago) : ICommand<bool>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.PagamentosAluguel.Commands
{
    public sealed record GerarPagamentoAluguelCommand(Guid LocacaoId, int Mes, int Ano, decimal ValorDevido, DateOnly DataVencimento) : ICommand<Guid>;
}

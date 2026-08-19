using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public sealed record FormalizarContratoLocacaoCommand(
        Guid Id,
        decimal ValorAluguel,
        DateOnly DataInicial,
        DateOnly DataFinal,
        int DiaVencimento,
        decimal Garantia,
        decimal TaxaAdministracao,
        IndiceReajuste IndiceReajuste) : ICommand<bool>;
}

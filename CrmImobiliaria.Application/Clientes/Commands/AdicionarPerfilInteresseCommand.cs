using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed record AdicionarPerfilInteresseCommand(
        Guid ClienteId,
        TipoNegociacaoImovel TipoNegociacao,
        TipoImovel TipoImovel,
        string? LocalizacaoDesejada,
        decimal ValorMinimo,
        decimal ValorMaximo,
        int? NumeroQuartos,
        FormaPagamento FormaPagamento,
        string? Observacoes) : ICommand<bool>;
}

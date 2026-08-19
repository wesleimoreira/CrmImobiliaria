using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Propostas.Commands
{
    // Só propostas de imóvel avulso por enquanto — proposta de lote entra junto com o módulo de Loteamentos.
    public sealed record CriarPropostaCommand(
        Guid ClienteId,
        Guid ImovelId,
        Guid AnuncioImovelId,
        Guid CorretorId,
        decimal ValorProposto,
        decimal? Entrada,
        int? NumeroParcelas,
        decimal? ValorParcela,
        FormaPagamento FormaPagamento) : ICommand<Guid>;
}

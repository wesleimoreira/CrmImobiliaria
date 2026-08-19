using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Lotes.Commands
{
    public sealed record CriarLoteCommand(
        Guid EmpreendimentoId,
        string? Quadra,
        string? Numero,
        decimal AreaM2,
        decimal Valor,
        decimal EntradaMinima,
        int ParcelamentoMaximo,
        decimal? ValorPromocional) : ICommand<Guid>;
}

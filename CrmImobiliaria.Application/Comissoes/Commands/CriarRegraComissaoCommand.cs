using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Comissoes.Commands
{
    public sealed record CriarRegraComissaoCommand(
        string Nome,
        decimal PercentualComissaoTotal,
        Guid? ImovelId,
        Dictionary<PapelComissao, decimal> Rateio) : ICommand<Guid>;
}

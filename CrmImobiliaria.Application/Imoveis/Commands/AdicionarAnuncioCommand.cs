using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed record AdicionarAnuncioCommand(
        Guid ImovelId,
        TipoNegociacaoImovel Tipo,
        decimal Valor,
        bool Exclusividade,
        int? EstadiaMinimaNoites) : ICommand<Guid>;
}

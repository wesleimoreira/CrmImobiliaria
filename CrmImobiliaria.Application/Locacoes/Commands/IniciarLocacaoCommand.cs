using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Locacoes.Commands
{
    public sealed record IniciarLocacaoCommand(
        Guid ProprietarioId, Guid LocatarioId, Guid ImovelId, Guid AnuncioImovelId, Guid CorretorId, Guid? LeadId) : ICommand<Guid>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed record AgendarVistoriaCommand(Guid LocacaoId, Guid ImovelId, TipoVistoria Tipo, DateTime DataAgendada, Guid ResponsavelId) : ICommand<Guid>;
}

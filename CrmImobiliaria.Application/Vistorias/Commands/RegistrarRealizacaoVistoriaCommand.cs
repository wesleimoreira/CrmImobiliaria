using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Vistorias.Commands
{
    public sealed record RegistrarRealizacaoVistoriaCommand(Guid Id, DateTime DataRealizada, string? Observacoes) : ICommand<bool>;
}

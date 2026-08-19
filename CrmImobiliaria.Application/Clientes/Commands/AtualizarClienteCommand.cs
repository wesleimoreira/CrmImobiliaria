using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed record AtualizarClienteCommand(
        Guid Id,
        string Telefone,
        string? WhatsApp,
        string Email,
        List<TipoCliente> Tipos,
        Guid CorretorResponsavelId,
        string? Documento,
        string? Observacoes) : ICommand<bool>;
}

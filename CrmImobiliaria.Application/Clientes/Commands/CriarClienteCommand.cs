using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Clientes.Commands
{
    public sealed record CriarClienteCommand(
        string Nome,
        string Telefone,
        string? WhatsApp,
        string Email,
        List<TipoCliente> Tipos,
        Guid CorretorResponsavelId,
        OrigemCliente Origem,
        string? Documento,
        string? CampanhaEspecifica,
        string? Observacoes) : ICommand<Guid>;
}

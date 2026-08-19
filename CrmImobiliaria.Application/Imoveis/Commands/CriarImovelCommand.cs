using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed record CriarImovelCommand(
        Guid ProprietarioId,
        Guid CorretorCaptadorId,
        TipoImovel Tipo,
        string Logradouro,
        string Numero,
        string? Complemento,
        string Bairro,
        string Cidade,
        string Uf,
        string Cep,
        decimal AreaM2,
        int Quartos,
        int Suites,
        int Garagem) : ICommand<Guid>;
}

using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Imoveis.Commands
{
    public sealed record AtualizarImovelCommand(
        Guid Id,
        Guid CorretorCaptadorId,
        string Logradouro,
        string Numero,
        string? Complemento,
        string Bairro,
        string Cidade,
        string Uf,
        string Cep,
        int Quartos,
        int Suites,
        int Garagem,
        List<string> Caracteristicas) : ICommand<bool>;
}

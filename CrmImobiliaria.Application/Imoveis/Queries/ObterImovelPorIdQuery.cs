using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Imoveis.Queries
{
    public sealed record ObterImovelPorIdQuery(Guid Id) : IQuery<ImovelDetalheDto?>;

    public sealed record ImovelDetalheDto(
        Guid Id,
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
        int Garagem,
        List<string> Caracteristicas,
        List<AnuncioResumoDto> Anuncios);

    public sealed record AnuncioResumoDto(Guid Id, TipoNegociacaoImovel Tipo, string Codigo, decimal Valor, StatusAnuncio Status, bool Exclusividade);
}

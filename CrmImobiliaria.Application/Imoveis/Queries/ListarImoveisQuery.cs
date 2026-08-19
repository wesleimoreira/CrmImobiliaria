using CrmImobiliaria.Application.Abstractions.Messaging;

namespace CrmImobiliaria.Application.Imoveis.Queries
{
    public sealed record ListarImoveisQuery(string? Termo) : IQuery<List<ImovelListaItemDto>>;

    public sealed record ImovelListaItemDto(Guid Id, string Endereco, string Tipo, string ProprietarioNome, string CorretorNome, int QtdAnuncios);
}

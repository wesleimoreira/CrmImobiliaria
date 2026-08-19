using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Imoveis.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ObterImovelPorIdQueryHandler(CrmDbContext context) : IQueryHandler<ObterImovelPorIdQuery, ImovelDetalheDto?>
    {
        public async Task<Result<ImovelDetalheDto?>> HandleAsync(ObterImovelPorIdQuery query, CancellationToken cancellationToken = default)
        {
            var imovel = await context.Imoveis.AsNoTracking().FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);
            if (imovel is null)
                return Result<ImovelDetalheDto?>.Success(null);

            var anuncios = imovel.Anuncios
                .Select(a => new AnuncioResumoDto(a.Id, a.Tipo, a.Codigo.ToString()!, a.Valor.Valor, a.Status, a.Exclusividade))
                .ToList();

            var dto = new ImovelDetalheDto(
                imovel.Id, imovel.ProprietarioId, imovel.CorretorCaptadorId, imovel.Tipo,
                imovel.Endereco.Logradouro, imovel.Endereco.Numero, imovel.Endereco.Complemento, imovel.Endereco.Bairro,
                imovel.Endereco.Cidade, imovel.Endereco.Uf, imovel.Endereco.Cep, imovel.Area.MetrosQuadrados,
                imovel.Quartos, imovel.Suites, imovel.Garagem, imovel.Caracteristicas.ToList(), anuncios);

            return Result<ImovelDetalheDto?>.Success(dto);
        }
    }
}

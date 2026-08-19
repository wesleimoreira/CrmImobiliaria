using CrmImobiliaria.Application.Abstractions.Messaging;
using CrmImobiliaria.Application.Imoveis.Queries;
using CrmImobiliaria.Shared;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Queries
{
    public sealed class ListarImoveisQueryHandler(CrmDbContext context) : IQueryHandler<ListarImoveisQuery, List<ImovelListaItemDto>>
    {
        public async Task<Result<List<ImovelListaItemDto>>> HandleAsync(ListarImoveisQuery query, CancellationToken cancellationToken = default)
        {
            var consulta = context.Imoveis.AsNoTracking()
                .Join(context.Clientes.AsNoTracking(), i => i.ProprietarioId, c => c.Id, (i, c) => new { Imovel = i, ProprietarioNome = c.Nome })
                .Join(context.Corretores.AsNoTracking(), x => x.Imovel.CorretorCaptadorId, cr => cr.Id, (x, cr) => new { x.Imovel, x.ProprietarioNome, CorretorNome = cr.Nome });

            if (!string.IsNullOrWhiteSpace(query.Termo))
                consulta = consulta.Where(x => x.Imovel.Endereco.Cidade.Contains(query.Termo) || x.Imovel.Endereco.Bairro.Contains(query.Termo));

            var brutos = await consulta
                .Select(x => new { x.Imovel.Id, x.Imovel.Endereco, x.Imovel.Tipo, x.ProprietarioNome, x.CorretorNome, QtdAnuncios = x.Imovel.Anuncios.Count })
                .ToListAsync(cancellationToken);

            var itens = brutos
                .Select(x => new ImovelListaItemDto(x.Id, x.Endereco.ToString(), x.Tipo.ToString(), x.ProprietarioNome, x.CorretorNome, x.QtdAnuncios))
                .ToList();

            return Result<List<ImovelListaItemDto>>.Success(itens);
        }
    }
}

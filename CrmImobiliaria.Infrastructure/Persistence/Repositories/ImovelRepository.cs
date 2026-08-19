using CrmImobiliaria.Application.Abstractions.Persistence;
using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrmImobiliaria.Infrastructure.Persistence.Repositories
{
    public sealed class ImovelRepository(CrmDbContext context) : IImovelRepository
    {
        public Task<Imovel?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Imoveis.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        public Task<Imovel?> ObterPorAnuncioIdAsync(Guid anuncioImovelId, CancellationToken cancellationToken = default) =>
            context.Imoveis.FirstOrDefaultAsync(i => i.Anuncios.Any(a => a.Id == anuncioImovelId), cancellationToken);

        public async Task<int> ContarAnunciosPorTipoEAnoAsync(TipoNegociacaoImovel tipo, int ano, CancellationToken cancellationToken = default)
        {
            // Codigo é mapeado como coluna única (conversor), então não dá pra filtrar por Codigo.Ano
            // direto na query — traz os códigos do tipo (isso sim traduzível) e filtra por ano em memória.
            var codigos = await context.Imoveis.SelectMany(i => i.Anuncios)
                .Where(a => a.Tipo == tipo)
                .Select(a => a.Codigo)
                .ToListAsync(cancellationToken);

            return codigos.Count(c => c.Ano == ano);
        }

        public void Adicionar(Imovel entidade) => context.Imoveis.Add(entidade);
        public void Remover(Imovel entidade) => context.Imoveis.Remove(entidade);
    }
}

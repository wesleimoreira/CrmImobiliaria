using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IImovelRepository : IRepository<Imovel>
    {
        Task<Imovel?> ObterPorAnuncioIdAsync(Guid anuncioImovelId, CancellationToken cancellationToken = default);

        // Usado só pra gerar o próximo CodigoImovel (V/L/T-ANO-SEQUENCIAL) ao criar um anúncio.
        Task<int> ContarAnunciosPorTipoEAnoAsync(TipoNegociacaoImovel tipo, int ano, CancellationToken cancellationToken = default);
    }
}

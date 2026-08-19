using CrmImobiliaria.Domain.Entities;

namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IVendaRepository : IRepository<Venda>
    {
        Task<bool> ExisteParaPropostaAsync(Guid propostaId, CancellationToken cancellationToken = default);
    }
}

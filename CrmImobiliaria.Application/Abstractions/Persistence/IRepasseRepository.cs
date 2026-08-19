using CrmImobiliaria.Domain.Entities;

namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IRepasseRepository : IRepository<Repasse>
    {
        Task<bool> ExisteParaPagamentoAsync(Guid pagamentoAluguelId, CancellationToken cancellationToken = default);
    }
}

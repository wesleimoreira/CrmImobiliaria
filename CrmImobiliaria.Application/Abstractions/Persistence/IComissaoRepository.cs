using CrmImobiliaria.Domain.Entities;
using CrmImobiliaria.Domain.Enums;

namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IComissaoRepository : IRepository<Comissao>
    {
        Task<bool> ExisteParaOrigemAsync(OrigemComissao origemTipo, Guid origemId, CancellationToken cancellationToken = default);
    }
}

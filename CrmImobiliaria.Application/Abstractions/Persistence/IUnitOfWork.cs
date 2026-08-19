namespace CrmImobiliaria.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SalvarAsync(CancellationToken cancellationToken = default);
    }
}

using PersonManager.Repositories;

namespace PersonManager.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPersonRepository PersonRepository { get; }
        IAddressRepository AddressRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IProjectRepository ProjectRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}

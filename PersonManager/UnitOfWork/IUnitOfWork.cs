using MediatRApp.Repositories;

namespace MediatRApp.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPersonRepository PersonRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}

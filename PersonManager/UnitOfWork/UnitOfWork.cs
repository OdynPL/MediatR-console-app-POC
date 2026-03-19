using MediatRApp.Data;
using MediatRApp.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace MediatRApp.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _transaction;
        public IPersonRepository PersonRepository { get; }
        public UnitOfWork(AppDbContext db, IPersonRepository personRepository)
        {
            _db = db;
            PersonRepository = personRepository;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await BeginTransactionAsync(cancellationToken);
                var result = await _db.SaveChangesAsync(cancellationToken);
                await CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                _transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}

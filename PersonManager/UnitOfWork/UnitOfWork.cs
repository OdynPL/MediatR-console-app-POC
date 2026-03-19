using PersonManager.Data;
using PersonManager.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace PersonManager.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _transaction;
        public IPersonRepository PersonRepository { get; }
        public IAddressRepository AddressRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IProjectRepository ProjectRepository { get; }

        public UnitOfWork(
            AppDbContext db,
            IPersonRepository personRepository,
            IAddressRepository addressRepository,
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository)
        {
            _db = db;
            PersonRepository = personRepository;
            AddressRepository = addressRepository;
            CompanyRepository = companyRepository;
            ProjectRepository = projectRepository;
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

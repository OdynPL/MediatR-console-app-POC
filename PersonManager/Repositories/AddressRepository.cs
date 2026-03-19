using PersonManager.Data;
using PersonManager.Domain;
using PersonManager.DTO;
using Microsoft.EntityFrameworkCore;
using PersonManager.Specifications;

namespace PersonManager.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _db;
        public AddressRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RepositoryResult<Address>> AddAsync(Address address, CancellationToken cancellationToken = default)
        {
            if (address == null)
                return RepositoryResult<Address>.Fail("Address is null");
            try
            {
                await _db.Addresses.AddAsync(address, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return RepositoryResult<Address>.Ok(address);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Address>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<Address>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = await _db.Addresses.FindAsync(new object[] { id }, cancellationToken);
                if (address == null)
                    return RepositoryResult<Address>.Fail("Address not found");
                return RepositoryResult<Address>.Ok(address);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Address>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<List<Address>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var addresses = await _db.Addresses.AsQueryable().ToListAsync(cancellationToken);
                return RepositoryResult<List<Address>>.Ok(addresses);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Address>>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<Address>> UpdateAsync(Address address, CancellationToken cancellationToken = default)
        {
            if (address == null)
                return RepositoryResult<Address>.Fail("Address is null");
            try
            {
                _db.Addresses.Update(address);
                await _db.SaveChangesAsync(cancellationToken);
                return RepositoryResult<Address>.Ok(address);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Address>.Fail(ex.Message);
            }
        }
        public async Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = await _db.Addresses.FindAsync(new object[] { id }, cancellationToken);
                if (address == null)
                    return RepositoryResult<bool>.Fail("Address not found");
                _db.Addresses.Remove(address);
                await _db.SaveChangesAsync(cancellationToken);
                return RepositoryResult<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Fail(ex.Message);
            }
        }
        public IQueryable<Address> GetQueryable()
        {
            return _db.Addresses.AsQueryable();
        }

        public async Task<RepositoryResult<List<Address>>> GetBySpecificationAsync(ISpecification<Address> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                var addresses = await _db.Addresses
                    .Where(specification.Criteria)
                    .ToListAsync(cancellationToken);
                return RepositoryResult<List<Address>>.Ok(addresses);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Address>>.Fail(ex.Message);
            }
        }
    }
}

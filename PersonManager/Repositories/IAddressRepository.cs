using PersonManager.Domain;
using PersonManager.DTO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PersonManager.Repositories
{
    public interface IAddressRepository
    {
        Task<RepositoryResult<Address>> AddAsync(Address address, CancellationToken cancellationToken = default);
        Task<RepositoryResult<Address>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepositoryResult<List<Address>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<RepositoryResult<Address>> UpdateAsync(Address address, CancellationToken cancellationToken = default);
        Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Address> GetQueryable();
    }
}

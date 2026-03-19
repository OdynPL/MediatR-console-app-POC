
using PersonManager.Domain;

namespace PersonManager.Services
{
    public interface IAddressService
    {
        Task<DTO.RepositoryResult<Address>> AddAddressAsync(Address address, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<Address>> GetAddressByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<List<Address>>> GetAllAddressesAsync(CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<Address>> UpdateAddressAsync(Address address, CancellationToken cancellationToken = default);
        Task<DTO.RepositoryResult<bool>> DeleteAddressAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Address> GetQueryableAddresses();
    }
}

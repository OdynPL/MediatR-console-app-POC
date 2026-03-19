using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RepositoryResult<Address>> AddAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.AddressRepository.AddAsync(address, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<RepositoryResult<Address>> GetAddressByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.AddressRepository.GetByIdAsync(id, cancellationToken);
        }
        public async Task<RepositoryResult<List<Address>>> GetAllAddressesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.AddressRepository.GetAllAsync(cancellationToken);
        }
        public async Task<RepositoryResult<Address>> UpdateAddressAsync(Address address, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.AddressRepository.UpdateAsync(address, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<RepositoryResult<bool>> DeleteAddressAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.AddressRepository.DeleteAsync(id, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public IQueryable<Address> GetQueryableAddresses()
        {
            return _unitOfWork.AddressRepository.GetQueryable();
        }
    }
}

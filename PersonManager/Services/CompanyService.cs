using PersonManager.Domain;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DTO.RepositoryResult<Company>> AddCompanyAsync(Company company, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.CompanyRepository.AddAsync(company, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<DTO.RepositoryResult<Company>> GetCompanyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.CompanyRepository.GetByIdAsync(id, cancellationToken);
        }
        public async Task<DTO.RepositoryResult<List<Company>>> GetAllCompaniesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.CompanyRepository.GetAllAsync(cancellationToken);
        }
        public async Task<DTO.RepositoryResult<Company>> UpdateCompanyAsync(Company company, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.CompanyRepository.UpdateAsync(company, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<DTO.RepositoryResult<bool>> DeleteCompanyAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.CompanyRepository.DeleteAsync(id, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public IQueryable<Company> GetQueryableCompanies()
        {
            return _unitOfWork.CompanyRepository.GetQueryable();
        }
    }
}

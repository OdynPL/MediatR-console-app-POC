using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        public CompanyService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<RepositoryResult<Company>> AddCompanyAsync(Company company, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Dodawanie firmy: {company.Name}");
            var result = await _unitOfWork.CompanyRepository.AddAsync(company, cancellationToken);
            if (!result.Success)
            {
                _logger.LogWarning($"Nieudane dodanie firmy: {company.Name}");
                return result;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInfo($"Firma dodana: {company.Name}");
            return result;
        }
        public async Task<RepositoryResult<Company>> GetCompanyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.CompanyRepository.GetByIdAsync(id, cancellationToken);
        }
        public async Task<RepositoryResult<List<Company>>> GetAllCompaniesAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.CompanyRepository.GetAllAsync(cancellationToken);
        }
        public async Task<RepositoryResult<Company>> UpdateCompanyAsync(Company company, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.CompanyRepository.UpdateAsync(company, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<RepositoryResult<bool>> DeleteCompanyAsync(int id, CancellationToken cancellationToken = default)
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

        public async Task<int> CreateCompanyAsync(string name, CancellationToken cancellationToken = default)
        {
            var company = new Company { Name = name, Employees = new List<Person>() };
            var result = await _unitOfWork.CompanyRepository.AddAsync(company, cancellationToken);
            if (!result.Success || result.Data == null)
                return 0;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result.Data.Id;
        }
    }
}

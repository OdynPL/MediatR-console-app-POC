using PersonManager.Domain;
using PersonManager.DTO;

namespace PersonManager.Services
{
    public interface ICompanyService
    {
        Task<RepositoryResult<Company>> AddCompanyAsync(Company company, CancellationToken cancellationToken = default);
        Task<RepositoryResult<Company>> GetCompanyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepositoryResult<List<Company>>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
        Task<RepositoryResult<Company>> UpdateCompanyAsync(Company company, CancellationToken cancellationToken = default);
        Task<RepositoryResult<bool>> DeleteCompanyAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Company> GetQueryableCompanies();
    }
}

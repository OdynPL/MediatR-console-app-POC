using PersonManager.Domain;
using PersonManager.DTO;

namespace PersonManager.Repositories
{
    public interface ICompanyRepository
    {
        Task<RepositoryResult<Company>> AddAsync(Company company, CancellationToken cancellationToken = default);
        Task<RepositoryResult<Company>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepositoryResult<List<Company>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<RepositoryResult<Company>> UpdateAsync(Company company, CancellationToken cancellationToken = default);
        Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Company> GetQueryable();
    }
}

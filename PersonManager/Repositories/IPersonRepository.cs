
using PersonManager.Domain;
using PersonManager.DTO;

namespace PersonManager.Repositories
{
    public interface IPersonRepository
    {
        Task<RepositoryResult<Person>> AddAsync(Person person, CancellationToken cancellationToken = default);
        Task<RepositoryResult<Person>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepositoryResult<List<Person>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<RepositoryResult<Person>> UpdateAsync(Person person, CancellationToken cancellationToken = default);
        Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Person> GetQueryable();
    }
}

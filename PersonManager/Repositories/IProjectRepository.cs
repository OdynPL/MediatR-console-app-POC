using PersonManager.Domain;
using PersonManager.DTO;

public interface IProjectRepository
{
    Task<RepositoryResult<Project>> AddAsync(Project project, CancellationToken cancellationToken = default);
    Task<RepositoryResult<Project>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RepositoryResult<List<Project>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RepositoryResult<Project>> UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task<RepositoryResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    IQueryable<Project> GetQueryable();
}


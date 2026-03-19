using PersonManager.Domain;
using PersonManager.DTO;

namespace PersonManager.Services
{
    public interface IProjectService
    {
        Task<RepositoryResult<Project>> AddProjectAsync(Project project, CancellationToken cancellationToken = default);
        Task<RepositoryResult<Project>> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepositoryResult<List<Project>>> GetAllProjectsAsync(CancellationToken cancellationToken = default);
        Task<RepositoryResult<Project>> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default);
        Task<RepositoryResult<bool>> DeleteProjectAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<Project> GetQueryableProjects();
    }
}
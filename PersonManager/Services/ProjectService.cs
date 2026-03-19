using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.UnitOfWork;

namespace PersonManager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DTO.RepositoryResult<Project>> AddProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ProjectRepository.AddAsync(project, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<DTO.RepositoryResult<Project>> GetProjectByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ProjectRepository.GetByIdAsync(id, cancellationToken);
        }
        public async Task<DTO.RepositoryResult<List<Project>>> GetAllProjectsAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ProjectRepository.GetAllAsync(cancellationToken);
        }
        public async Task<DTO.RepositoryResult<Project>> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ProjectRepository.UpdateAsync(project, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public async Task<DTO.RepositoryResult<bool>> DeleteProjectAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.ProjectRepository.DeleteAsync(id, cancellationToken);
            if (!result.Success)
                return result;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        public IQueryable<Project> GetQueryableProjects()
        {
            return _unitOfWork.ProjectRepository.GetQueryable();
        }
    }
}

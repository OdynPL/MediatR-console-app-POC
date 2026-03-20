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
        private readonly ILoggerService _logger;
        public ProjectService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<DTO.RepositoryResult<Project>> AddProjectAsync(Project project, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Dodawanie projektu: {project.Title}");
            var result = await _unitOfWork.ProjectRepository.AddAsync(project, cancellationToken);
            if (!result.Success)
            {
                _logger.LogWarning($"Nieudane dodanie projektu: {project.Title}");
                return result;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInfo($"Projekt dodany: {project.Title}");
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

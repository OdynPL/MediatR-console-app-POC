using MediatR;
using PersonManager.Commands;
using PersonManager.Services;
using PersonManager.Domain;

namespace PersonManager.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectService _projectService;
        public UpdateProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProjectByIdAsync(request.Id, cancellationToken);
            if (!project.Success || project.Data == null)
                return false;
            project.Data.Title = request.Title;
            // Aktualizuj członków
            // ...implementacja zależna od serwisu...
            var result = await _projectService.UpdateProjectAsync(project.Data, cancellationToken);
            return result.Success;
        }
    }
}

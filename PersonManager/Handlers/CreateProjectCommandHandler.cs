using MediatR;
using PersonManager.Commands;
using PersonManager.Services;
using PersonManager.Domain;

namespace PersonManager.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectService _projectService;
        public CreateProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Title = request.Title,
                Members = new List<Person>()
            };
            // Dodaj osoby do projektu
            // ...implementacja zależna od serwisu...
            var result = await _projectService.AddProjectAsync(project, cancellationToken);
            return result.Success && result.Data != null ? result.Data.Id : 0;
        }
    }
}

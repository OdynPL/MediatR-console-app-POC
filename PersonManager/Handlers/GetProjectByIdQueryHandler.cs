using MediatR;
using PersonManager.Queries;
using PersonManager.Services;
using PersonManager.Domain;

namespace PersonManager.Handlers
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Project>
    {
        private readonly IProjectService _projectService;
        public GetProjectByIdQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public async Task<Project> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _projectService.GetProjectByIdAsync(request.Id, cancellationToken);
            return result.Data;
        }
    }
}

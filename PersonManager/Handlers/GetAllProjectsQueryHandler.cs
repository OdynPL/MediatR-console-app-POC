using MediatR;
using PersonManager.Queries;
using PersonManager.Services;
using PersonManager.Domain;
using System.Collections.Generic;

namespace PersonManager.Handlers
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<Project>>
    {
        private readonly IProjectService _projectService;
        public GetAllProjectsQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public async Task<List<Project>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var result = await _projectService.GetAllProjectsAsync(cancellationToken);
            return result.Data ?? new List<Project>();
        }
    }
}

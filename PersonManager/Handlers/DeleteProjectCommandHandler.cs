using MediatR;
using PersonManager.Commands;
using PersonManager.Services;

namespace PersonManager.Handlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectService _projectService;
        public DeleteProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var result = await _projectService.DeleteProjectAsync(request.Id, cancellationToken);
            return result.Success;
        }
    }
}

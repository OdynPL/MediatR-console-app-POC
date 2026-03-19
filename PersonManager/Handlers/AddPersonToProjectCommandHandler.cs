using MediatR;
using PersonManager.Commands;
using PersonManager.Services;

namespace PersonManager.Handlers
{
    public class AddPersonToProjectCommandHandler : IRequestHandler<AddPersonToProjectCommand, bool>
    {
        private readonly IProjectService _projectService;
        private readonly IPersonService _personService;
        public AddPersonToProjectCommandHandler(IProjectService projectService, IPersonService personService)
        {
            _projectService = projectService;
            _personService = personService;
        }
        public async Task<bool> Handle(AddPersonToProjectCommand request, CancellationToken cancellationToken)
        {
            var personResult = await _personService.GetPersonByIdAsync(request.PersonId, cancellationToken);
            var projectResult = await _projectService.GetProjectByIdAsync(request.ProjectId, cancellationToken);
            if (!personResult.Success || personResult.Data == null || !projectResult.Success || projectResult.Data == null)
                return false;
            projectResult.Data.Members.Add(personResult.Data);
            var updateResult = await _projectService.UpdateProjectAsync(projectResult.Data, cancellationToken);
            return updateResult.Success;
        }
    }
}

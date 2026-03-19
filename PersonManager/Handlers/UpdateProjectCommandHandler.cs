using MediatR;
using PersonManager.Commands;
using PersonManager.Services;
using PersonManager.Domain;

namespace PersonManager.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectService _projectService;
        private readonly IPersonService _personService;
        public UpdateProjectCommandHandler(IProjectService projectService, IPersonService personService)
        {
            _projectService = projectService;
            _personService = personService;
        }
        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetProjectByIdAsync(request.Id, cancellationToken);
            if (!project.Success || project.Data == null)
                return false;
            project.Data.Title = request.Title;

            // Aktualizuj członków projektu
            var members = new List<Person>();
            foreach (var memberId in request.MemberIds)
            {
                var personResult = await _personService.GetPersonByIdAsync(memberId, cancellationToken);
                if (personResult.Success && personResult.Data != null)
                    members.Add(personResult.Data);
            }
            project.Data.Members = members;

            var result = await _projectService.UpdateProjectAsync(project.Data, cancellationToken);
            return result.Success;
        }
    }
}

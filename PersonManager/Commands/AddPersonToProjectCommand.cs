using MediatR;

namespace PersonManager.Commands
{
    public class AddPersonToProjectCommand : IRequest<bool>
    {
        public int PersonId { get; set; }
        public int ProjectId { get; set; }
    }
}

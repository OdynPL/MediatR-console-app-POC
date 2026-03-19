using PersonManager.Domain;
using MediatR;

namespace PersonManager.Commands
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required List<int> MemberIds { get; set; }
    }
}

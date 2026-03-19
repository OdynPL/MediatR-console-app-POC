using PersonManager.Domain;
using MediatR;

namespace PersonManager.Commands
{
    public class CreateProjectCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public required List<int> MemberIds { get; set; }
    }
}

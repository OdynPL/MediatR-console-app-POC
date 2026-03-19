using PersonManager.Domain;
using MediatR;

namespace PersonManager.Commands
{
    public class CreateProjectCommand : IRequest<int>
    {
        public string Title { get; set; }
        public List<int> MemberIds { get; set; }
    }
}

using PersonManager.Domain;
using MediatR;

namespace PersonManager.Commands
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> MemberIds { get; set; }
    }
}

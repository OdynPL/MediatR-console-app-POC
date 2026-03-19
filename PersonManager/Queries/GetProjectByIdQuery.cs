using MediatR;
using PersonManager.Domain;

namespace PersonManager.Queries
{
    public class GetProjectByIdQuery : IRequest<Project>
    {
        public int Id { get; set; }
    }
}

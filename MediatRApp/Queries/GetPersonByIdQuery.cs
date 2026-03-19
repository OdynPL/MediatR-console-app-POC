using MediatR;
using MediatRApp.DTO;

namespace MediatRApp.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonResponseDto>
    {
        public int Id { get; set; }
        public GetPersonByIdQuery(int id)
        {
            Id = id;
        }
    }
}
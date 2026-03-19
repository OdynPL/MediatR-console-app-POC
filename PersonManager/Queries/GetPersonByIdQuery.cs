using MediatR;
using PersonManager.DTO;

namespace PersonManager.Queries
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
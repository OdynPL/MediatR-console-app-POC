using MediatR;
using PersonManager.DTO;

namespace PersonManager.Commands
{
    public class UpdatePersonCommand : IRequest<PersonResponseDto>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
    }
}
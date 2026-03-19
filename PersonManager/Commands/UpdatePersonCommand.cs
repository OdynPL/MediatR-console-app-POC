using MediatR;
using MediatRApp.DTO;

namespace MediatRApp.Commands
{
    public class UpdatePersonCommand : IRequest<PersonResponseDto>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
    }
}
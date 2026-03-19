using MediatR;
using MediatRApp.DTO;

namespace MediatRApp.Commands
{
    public class CreatePersonCommand : IRequest<PersonResponseDto>
    {
        public string Name { get; }
        public int Age { get; }
        public CreatePersonCommand(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}

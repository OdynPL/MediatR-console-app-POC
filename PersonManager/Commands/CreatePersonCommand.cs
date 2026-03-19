using MediatR;
using PersonManager.DTO;

namespace PersonManager.Commands
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

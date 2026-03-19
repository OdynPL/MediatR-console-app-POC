using MediatR;
using PersonManager.DTO;

namespace PersonManager.Commands
{
    public class CreatePersonCommand : IRequest<PersonResponseDto>
    {
        public string Name { get; }
        public int Age { get; }
        public int? AddressId { get; }
        public int? CompanyId { get; }

        public CreatePersonCommand(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public CreatePersonCommand(string name, int age, int? addressId, int? companyId)
        {
            Name = name;
            Age = age;
            AddressId = addressId;
            CompanyId = companyId;
        }
    }
}

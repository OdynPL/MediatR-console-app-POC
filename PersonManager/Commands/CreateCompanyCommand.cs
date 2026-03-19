using MediatR;

namespace PersonManager.Commands
{
    public class CreateCompanyCommand : IRequest<int>
    {
        public required string Name { get; set; }
    }
}

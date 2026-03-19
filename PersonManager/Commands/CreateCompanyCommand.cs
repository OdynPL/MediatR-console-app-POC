using MediatR;

namespace PersonManager.Commands
{
    public class CreateCompanyCommand : IRequest<int>
    {
        public string Name { get; set; }
    }
}

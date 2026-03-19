using MediatR;

namespace PersonManager.Commands
{
    public class CreateAddressCommand : IRequest<int>
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}

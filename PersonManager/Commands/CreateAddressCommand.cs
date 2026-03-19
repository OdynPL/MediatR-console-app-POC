using MediatR;

namespace PersonManager.Commands
{
    public class CreateAddressCommand : IRequest<int>
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
    }
}

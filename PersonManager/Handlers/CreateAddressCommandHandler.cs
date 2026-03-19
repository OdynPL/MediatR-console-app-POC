using MediatR;
using PersonManager.Commands;
using PersonManager.Services;

namespace PersonManager.Handlers
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, int>
    {
        private readonly IAddressService _addressService;
        public CreateAddressCommandHandler(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            return await _addressService.CreateAddressAsync(request.Street, request.City, request.Country, cancellationToken);
        }
    }
}

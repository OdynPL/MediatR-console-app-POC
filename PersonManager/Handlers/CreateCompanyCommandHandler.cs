using MediatR;
using PersonManager.Commands;
using PersonManager.Services;

namespace PersonManager.Handlers
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
    {
        private readonly ICompanyService _companyService;
        public CreateCompanyCommandHandler(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            return await _companyService.CreateCompanyAsync(request.Name);
        }
    }
}

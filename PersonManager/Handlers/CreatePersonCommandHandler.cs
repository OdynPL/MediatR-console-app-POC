using AutoMapper;
using MediatR;
using PersonManager.Commands;
using PersonManager.Domain;
using PersonManager.DTO;
using PersonManager.Services;
namespace PersonManager.Handlers
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonResponseDto>
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        public CreatePersonCommandHandler(IMapper mapper, IPersonService personService)
        {
            _mapper = mapper;
            _personService = personService;
        }
        public async Task<PersonResponseDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var response = new PersonResponseDto();
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Success = false;
                response.ErrorMessage = GlobalConfig.PersonNameEmptyError;
                return response;
            }
            if (request.Age <= 0)
            {
                response.Success = false;
                response.ErrorMessage = GlobalConfig.PersonAgePositiveError;
                return response;
            }
            try
            {
                var person = _mapper.Map<Person>(request);
                await _personService.AddPersonAsync(person, cancellationToken);
                response = _mapper.Map<PersonResponseDto>(person);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}

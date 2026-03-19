using MediatR;
using PersonManager.Commands;
using PersonManager.Repositories;
using PersonManager.DTO;
using AutoMapper;

namespace PersonManager.Handlers
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonResponseDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        public UpdatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponseDto> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (!result.Success || result.Data == null)
            {
                return new PersonResponseDto { Success = false, ErrorMessage = result.ErrorMessage ?? "Person not found" };
            }
            var person = result.Data;
            _mapper.Map(request, person); // mapuje tylko Name i Age
            var updateResult = await _repository.UpdateAsync(person, cancellationToken);
            if (!updateResult.Success)
            {
                return new PersonResponseDto { Success = false, ErrorMessage = updateResult.ErrorMessage };
            }
            return new PersonResponseDto { Id = person.Id, Name = person.Name, Age = person.Age, Success = true };
        }
    }
}
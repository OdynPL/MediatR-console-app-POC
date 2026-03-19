using MediatR;
using MediatRApp.Queries;
using MediatRApp.Repositories;
using MediatRApp.DTO;
using AutoMapper;

namespace MediatRApp.Handlers
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonResponseDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        public GetPersonByIdQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PersonResponseDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (!result.Success || result.Data == null)
            {
                return new PersonResponseDto { Success = false, ErrorMessage = result.ErrorMessage ?? "Person not found" };
            }
            var person = result.Data;
            var dto = _mapper.Map<PersonResponseDto>(person);
            dto.Success = true;
            return dto;
        }
    }
}
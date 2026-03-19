using MediatR;
using PersonManager.Queries;
using PersonManager.Repositories;
using PersonManager.DTO;
using AutoMapper;

namespace PersonManager.Handlers
{
    public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, List<PersonResponseDto>>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        public GetAllPersonsQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<PersonResponseDto>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);
            if (!result.Success || result.Data == null)
            {
                return new List<PersonResponseDto>();
            }
            return result.Data.Select(p => _mapper.Map<PersonResponseDto>(p)).ToList();
        }
    }
}
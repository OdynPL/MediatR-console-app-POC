using AutoMapper;
using MediatRApp.Domain;
using MediatRApp.DTO;

namespace MediatRApp
{
    using MediatRApp.Commands;
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonResponseDto>();
            CreateMap<CreatePersonCommand, Person>();
            CreateMap<UpdatePersonCommand, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id nie powinien być nadpisywany
        }
    }
}

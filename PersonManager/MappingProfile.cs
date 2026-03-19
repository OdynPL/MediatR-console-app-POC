using AutoMapper;
using PersonManager.Domain;
using PersonManager.DTO;

namespace PersonManager
{
    using PersonManager.Commands;
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

using AutoMapper;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MyEvent, EventDto>().ReverseMap();
            CreateMap<MyEvent, EventWithDetailsDto>().ReverseMap();

            CreateMap<EventImage, EventImageDto>().ReverseMap();

            CreateMap<Member, MemberDto>().ReverseMap();

            CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}
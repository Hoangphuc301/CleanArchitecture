using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Infrastructure.Persistence.Mongo;

namespace CleanArchitecture.Infrastructure.Common.Mapping
{
    public class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
        {
            CreateMap<MenuLog, MenuDto>();
            CreateMap<NewLog, NewDTO>().ForMember(dest => dest.NewsId, opt => opt.MapFrom(src => src.NewId)); ;
        }
    }
}
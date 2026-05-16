using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Infrastructure.Persistence.Mongo;

namespace CleanArchitecture.Infrastructure.Common.Mapping
{
    public class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
        {
            CreateMap<MenuLog, MenuDto>();
        }
    }
}
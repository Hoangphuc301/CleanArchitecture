using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Application.Features.DTOs.New;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CleanArchitecture.Application.Features.Menu.Commands.CreateMenu;
using CleanArchitecture.Application.Features.New.Commands.CreateNew;
using CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu;
using CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu;
using CleanArchitecture.Application.Features.New.Commands.DeleteNew;
using CleanArchitecture.Application.Features.New.Commands.UpdateNew;

namespace CleanArchitecture.Application.Common.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // Menu mapping
            CreateMap<Menus, MenuDto>().ReverseMap();
            CreateMap<CreateMenuCommand, Menus>();
            CreateMap<DeleteMenuCommand, Menus>();
            CreateMap<UpdateMenuCommand, Menus>();

            // New mapping
            CreateMap<News, NewDTO>().ReverseMap();
            CreateMap<CreateNewCommand,News>();
            CreateMap<DeleteNewCommand, News>();
            CreateMap<UpdateNewCommand, News>();
        }
    }
}
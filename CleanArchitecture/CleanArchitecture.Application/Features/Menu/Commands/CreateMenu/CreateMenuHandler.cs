using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.CreateMenu
{
    public class CreateMenuHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public CreateMenuHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            var menuEntity = _mapper.Map<Menus>(request);
            var result = await _menuRepository.CreateAsync(menuEntity);
            return _mapper.Map<MenuDto>(result);
        }
    }
}

using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu
{
    public class UpdateMenuHandler : IRequestHandler<UpdateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public UpdateMenuHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetByIdAsync(request.MenuId);

            menu.MenuName = request.Name;
            menu.Slug = request.Slug;
            menu.DisplayOrder = (int?)request.DisplayOrder;

            await _menuRepository.UpdateAsync(menu.MenuId,menu);

            return _mapper.Map<MenuDto>(menu);
        }
    }
}

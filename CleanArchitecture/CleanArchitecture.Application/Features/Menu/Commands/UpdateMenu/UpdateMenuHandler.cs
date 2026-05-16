
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Application.Features.Menu.Events;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu
{
    public class UpdateMenuHandler : IRequestHandler<UpdateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public UpdateMenuHandler(IMenuRepository menuRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<MenuDto> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetByIdAsync(request.MenuId);

            menu.MenuName = request.Name;
            menu.Slug = request.Slug;
            menu.DisplayOrder = (int?)request.DisplayOrder;

            await _menuRepository.UpdateAsync(menu.MenuId,menu);

            await _messagePublisher.PublishAsync(
                exchange: "menu-exchange",
                routingKey: "menu.updated",
                message: new MenuUpdatedEvent
                {
                    MenuId = menu.MenuId,
                    NewMenuName = menu.MenuName,
                    Slug = menu.Slug,
                    DisplayOrder = menu.DisplayOrder
                });

            return _mapper.Map<MenuDto>(menu);
        }
    }
}

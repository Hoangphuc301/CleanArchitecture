using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Menu.Events;

namespace CleanArchitecture.Application.Features.Menu.Commands.CreateMenu
{
    public class CreateMenuHandler : IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public CreateMenuHandler(IMenuRepository menuRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<MenuDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            var menuEntity = _mapper.Map<Menus>(request);
            var result = await _menuRepository.CreateAsync(menuEntity);

            await _messagePublisher.PublishAsync("menu-exchange", "menu.created", new MenuCreatedEvent {
                MenuId = result.MenuId,
                MenuName = result.MenuName,
                Slug = result.Slug, 
                DisplayOrder = result.DisplayOrder
            });

            return _mapper.Map<MenuDto>(result);
        }
    }
}
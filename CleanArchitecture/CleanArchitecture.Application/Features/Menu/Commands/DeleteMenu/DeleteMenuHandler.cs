

using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Menu.Events;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu
{
    public class DeleteMenuHandler : IRequestHandler<DeleteMenuCommand, int>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;   
        private readonly IMessagePublisher _messagePublisher;

        public DeleteMenuHandler(IMenuRepository menuRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<int> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _menuRepository.DeleteAsync(request.MenuId);

            await _messagePublisher.PublishAsync(
                exchange: "menu-exchange",
                routingKey: "menu.deleted",
                message: new MenuDeletedEvent
                {
                    MenuId = request.MenuId
                });

            return result;
        }
    }
}

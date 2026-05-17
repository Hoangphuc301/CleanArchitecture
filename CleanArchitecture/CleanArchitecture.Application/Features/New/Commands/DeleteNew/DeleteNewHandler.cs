
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Menu.Events;
using CleanArchitecture.Application.Features.New.Events;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.DeleteNew
{
    public class DeleteNewHandler : IRequestHandler<DeleteNewCommand, int>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public DeleteNewHandler(INewRepository newRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _newRepository = newRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<int> Handle(DeleteNewCommand request, CancellationToken cancellationToken)
        {
            var result = await _newRepository.DeleteAsync(request.NewId);

            await _messagePublisher.PublishAsync(
                 exchange: "new-exchange",
                 routingKey: "new.deleted",
                 message: new NewDeletedEvent
                 {
                     NewId = request.NewId
                 });

            return result;
        }
    }
}

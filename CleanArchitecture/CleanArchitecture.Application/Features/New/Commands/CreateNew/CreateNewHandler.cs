using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Application.Features.New.Events;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.CreateNew
{
    public class CreateNewHandler : IRequestHandler<CreateNewCommand, NewDTO>   
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public CreateNewHandler (INewRepository newRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _newRepository = newRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<NewDTO> Handle(CreateNewCommand request, CancellationToken cancellationToken)
        {
            var newEntity = _mapper.Map<News>(request);
            var result = await _newRepository.CreateAsync(newEntity);

            await _messagePublisher.PublishAsync("new-exchange", "new.created", new NewCreatedEvent
            {
                    NewsId = result.NewsId,
                    Title = result.Title,
                    Slug = result.Slug, 
                    Content = result.Content
                });

            return _mapper.Map<NewDTO>(result);
        }
    }
}

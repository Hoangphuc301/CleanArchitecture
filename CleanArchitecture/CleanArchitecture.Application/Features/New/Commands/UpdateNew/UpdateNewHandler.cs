
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Application.Features.New.Events;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.UpdateNew
{
    public class UpdateNewHandler : IRequestHandler<UpdateNewCommand, NewDTO>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public UpdateNewHandler(INewRepository newRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _newRepository = newRepository;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<NewDTO> Handle(UpdateNewCommand request, CancellationToken cancellationToken)
        {
            var news = await _newRepository.GetByIdAsync(request.NewId);

            news.Title = request.Title;
            news.Slug = request.Slug;
            news.Summary = request.Summary;
            news.Content = request.Content;

            await _messagePublisher.PublishAsync(
                exchange: "new-exchange",
                routingKey: "new.updated",
                message: new NewUpdatedEvent
                {
                    NewId = news.NewsId,
                    NewTitle = news.Title,
                    Slug = news.Slug,
                    Content = news.Content
                });

            await _newRepository.UpdateAsync(news.NewsId, news);

            return _mapper.Map<NewDTO>(news);
        }
    }
}


using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.UpdateNew
{
    public class UpdateNewHandler : IRequestHandler<UpdateNewCommand, NewDTO>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;

        public UpdateNewHandler(INewRepository newRepository, IMapper mapper)
        {
            _newRepository = newRepository;
            _mapper = mapper;
        }

        public async Task<NewDTO> Handle(UpdateNewCommand request, CancellationToken cancellationToken)
        {
            var news = await _newRepository.GetByIdAsync(request.NewId);

            news.Title = request.Title;
            news.Slug = request.Slug;
            news.Summary = request.Summary;
            news.Content = request.Content;

            await _newRepository.UpdateAsync(news.NewsId, news);

            return _mapper.Map<NewDTO>(news);
        }
    }
}

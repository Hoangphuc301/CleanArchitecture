using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Domain.Interfaces;
using MediatR;


namespace CleanArchitecture.Application.Features.New.Queries.GetAllNew
{
    public class GetAllNewQueryHandler : IRequestHandler<GetAllNewQuery, List<NewDTO>>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;
        public GetAllNewQueryHandler(INewRepository newRepository, IMapper mapper)
        {
            _newRepository = newRepository;
            _mapper = mapper;
        }

        public async Task<List<NewDTO>> Handle(GetAllNewQuery query, CancellationToken cancellationToken)
        {
            var news = await _newRepository.GetAllAsync();
            return _mapper.Map<List<NewDTO>>(news);
        }
    }
}

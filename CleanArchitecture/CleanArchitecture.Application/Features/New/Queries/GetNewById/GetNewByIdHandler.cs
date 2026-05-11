
using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Queries.GetNewById
{
    public class GetNewByIdHandler : IRequestHandler<GetNewByIdQuery, NewDTO>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;

        public GetNewByIdHandler(INewRepository newRepository, IMapper mapper)
        {
            _newRepository = newRepository;
            _mapper = mapper;
        }

        public async Task<NewDTO> Handle(GetNewByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await _newRepository.GetByIdAsync(request.NewId);

            return _mapper.Map<NewDTO>(news);
        }
    }
}

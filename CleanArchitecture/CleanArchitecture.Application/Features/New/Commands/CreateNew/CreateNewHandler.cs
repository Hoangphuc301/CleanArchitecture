using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.CreateNew
{
    public class CreateNewHandler : IRequestHandler<CreateNewCommand, NewDTO>   
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;

        public CreateNewHandler (INewRepository newRepository, IMapper mapper)
        {
            _newRepository = newRepository;
            _mapper = mapper;
        }

        public async Task<NewDTO> Handle(CreateNewCommand request, CancellationToken cancellationToken)
        {
            var newEntity = _mapper.Map<News>(request);
            var result = await _newRepository.CreateAsync(newEntity);
            return _mapper.Map<NewDTO>(result);
        }
    }
}

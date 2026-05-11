
using AutoMapper;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.DeleteNew
{
    public class DeleteNewHandler : IRequestHandler<DeleteNewCommand, int>
    {
        private readonly INewRepository _newRepository;
        private readonly IMapper _mapper;

        public DeleteNewHandler(INewRepository newRepository, IMapper mapper)
        {
            _newRepository = newRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteNewCommand request, CancellationToken cancellationToken)
        {
            var result = await _newRepository.DeleteAsync(request.NewId);
            return result;
        }
    }
}

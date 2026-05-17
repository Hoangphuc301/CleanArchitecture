
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Queries.GetNewById
{
    public class GetNewByIdHandler : IRequestHandler<GetNewByIdQuery, NewDTO>
    {
        private readonly INewReadRepository _newReadRepository;

        public GetNewByIdHandler(INewReadRepository newReadRepository)
        {
            _newReadRepository = newReadRepository;
        }

        public async Task<NewDTO> Handle(GetNewByIdQuery request, CancellationToken cancellationToken)
        {
            return await _newReadRepository.GetByIdAsync(request.NewId, cancellationToken);
        }
    }
}

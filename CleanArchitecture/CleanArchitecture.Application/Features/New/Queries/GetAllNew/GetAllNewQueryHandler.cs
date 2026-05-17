using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.New;
using MediatR;


namespace CleanArchitecture.Application.Features.New.Queries.GetAllNew
{
    public class GetAllNewQueryHandler : IRequestHandler<GetAllNewQuery, List<NewDTO>>
    {
        private readonly INewReadRepository _newReadRepository;
        public GetAllNewQueryHandler(INewReadRepository newRepository)
        {
            _newReadRepository = newRepository;
        }

        public async Task<List<NewDTO>> Handle(GetAllNewQuery query, CancellationToken cancellationToken)
        {
            return await _newReadRepository.GetAllAsync(cancellationToken);
        }
    }
}

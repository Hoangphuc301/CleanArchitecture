using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.Menu;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetAllMenu
{
    public class GetAllMenuHandler : IRequestHandler<GetAllMenuQuery, List<MenuDto>>
    {
        private readonly IMenuReadRepository _menuReadRepository;

        public GetAllMenuHandler(IMenuReadRepository menuReadRepository)
        {
            _menuReadRepository = menuReadRepository;
        }

        public async Task<List<MenuDto>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            return await _menuReadRepository.GetAllAsync(cancellationToken);
        }
    }
}
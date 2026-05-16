using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.Menu;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetMenuById
{
    public class GetMenuByIdHandler : IRequestHandler<GetMenuByIdQuery, MenuDto>
    {
        private readonly IMenuReadRepository _menuReadRepository;

        public GetMenuByIdHandler(IMenuReadRepository menuReadRepository)
        {
            _menuReadRepository = menuReadRepository;
        }

        public async Task<MenuDto> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            var menuDto = await _menuReadRepository.GetByIdAsync(request.MenuId, cancellationToken);

            return menuDto;
        }
    }
}
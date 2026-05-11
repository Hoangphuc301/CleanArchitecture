
using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetMenuById
{
    public class GetMenuByIdHandler : IRequestHandler<GetMenuByIdQuery, MenuDto>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetMenuByIdHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepository.GetByIdAsync(request.MenuId);

            return _mapper.Map<MenuDto>(menu);
        }
    }
}

using AutoMapper;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetAllMenu
{
    public class GetAllMenuHandler : IRequestHandler<GetAllMenuQuery, List<MenuDto>>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public GetAllMenuHandler(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            var menus = await _menuRepository.GetAllAsync();
            return _mapper.Map<List<MenuDto>>(menus);
        }
    }
}
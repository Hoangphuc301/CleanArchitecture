
using CleanArchitecture.Application.Features.DTOs.Menu;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetMenuById
{
    public class GetMenuByIdQuery : IRequest<MenuDto>
    {
        public int MenuId { get; set; }
    }
}

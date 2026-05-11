using MediatR;
using CleanArchitecture.Application.Features.DTOs.Menu;

namespace CleanArchitecture.Application.Features.Menu.Queries.GetAllMenu
{
    public class GetAllMenuQuery : IRequest<List<MenuDto>>
    {

    }
}
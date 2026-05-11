
using CleanArchitecture.Application.Features.DTOs.Menu;
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu
{
    public record UpdateMenuCommand
    (
        int MenuId,
        string Name,
        string Slug,
        decimal DisplayOrder
    
     ) : IRequest<MenuDto>;
}

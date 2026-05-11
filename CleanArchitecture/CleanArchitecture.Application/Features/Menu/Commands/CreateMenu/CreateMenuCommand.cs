using MediatR;
using CleanArchitecture.Application.Features.DTOs.Menu;

namespace CleanArchitecture.Application.Features.Menu.Commands.CreateMenu
{
    public record CreateMenuCommand
    (
        string MenuName, 
        string? Slug, 
        int? DisplayOrder
    ) : IRequest<MenuDto>;
}
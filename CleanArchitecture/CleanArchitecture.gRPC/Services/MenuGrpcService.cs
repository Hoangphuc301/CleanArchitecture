using CleanArchitecture.Application.Features.Menu.Commands.CreateMenu;
using CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu;
using CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu;
using CleanArchitecture.Application.Features.Menu.Queries.GetAllMenu;
using CleanArchitecture.Application.Features.Menu.Queries.GetMenuById;
using CleanArchitecture.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace CleanArchitecture.Grpc.Services
{
    public class MenuGrpcService : MenuService.MenuServiceBase
    {
        private readonly IMediator _mediator;

        public MenuGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetAllMenusResponse> GetAllMenus(GetAllMenusRequest request, ServerCallContext context)
        {
            var menus = await _mediator.Send(new GetAllMenuQuery());

            var response = new GetAllMenusResponse();

            response.Items.AddRange(menus.Select(x => new MenuModel
            {
                Id = x.MenuId,
                MenuName = x.MenuName ?? string.Empty,
                Slug = x.Slug ?? string.Empty,
                DisplayOrder = x.DisplayOrder ?? 0
            }));

            return response;
        }

        public override async Task<GetMenuByIdResponse> GetMenuById(GetMenuByIdRequest request, ServerCallContext context)
        {
            var menusItem = await _mediator.Send(new GetMenuByIdQuery { MenuId = request.Id });

            if (menusItem == null)
            {
                return new GetMenuByIdResponse { Found = false };
            }

            return new GetMenuByIdResponse
            {
                Found = true,
                Menu = new MenuModel
                {
                    Id = menusItem.MenuId,
                    MenuName = menusItem.MenuName ?? string.Empty,
                    Slug = menusItem.Slug ?? string.Empty,
                    DisplayOrder = menusItem.DisplayOrder ?? 0
                }
            };
        }

        public override async Task<CreateMenuResponse> CreateMenu(CreateMenuRequest request, ServerCallContext context)
        {
            var savedMenu = await _mediator.Send(new CreateMenuCommand
            (
                request.MenuName,
                request.Slug,
                request.DisplayOrder
            ));

            return new CreateMenuResponse { Id = savedMenu?.MenuId ?? 0 };
        }

        public override async Task<UpdateMenuResponse> UpdateMenu(UpdateMenuRequest request, ServerCallContext context)
        {
            var result = await _mediator.Send(new UpdateMenuCommand
            (
                request.Id,
                request.MenuName,
                request.Slug,
                request.DisplayOrder
            ));

            return new UpdateMenuResponse { Success = result != null && result.MenuId > 0 };
        }

        public override async Task<DeleteMenuResponse> DeleteMenu(DeleteMenuRequest request, ServerCallContext context)
        {
            try
            {
                await _mediator.Send(new DeleteMenuCommand { MenuId = request.Id });
                return new DeleteMenuResponse { Success = true };
            }
            catch (Exception)
            {
                return new DeleteMenuResponse { Success = false };
            }
        }
    }
}
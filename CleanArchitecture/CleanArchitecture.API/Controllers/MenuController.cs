using Microsoft.AspNetCore.Mvc;
using MediatR;
using CleanArchitecture.Application.Features.Menu.Commands.CreateMenu;
using CleanArchitecture.Application.Features.Menu.Queries.GetAllMenu;
using CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu;
using CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu;
using CleanArchitecture.Application.Features.Menu.Queries.GetMenuById;


namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await _mediator.Send(new GetAllMenuQuery());
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMenuCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _mediator.Send(new DeleteMenuCommand { MenuId = id });
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMenuCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _mediator.Send(new GetMenuByIdQuery { MenuId = id });

            if (res == null)
            {
                return NotFound(new { message = $"Không tìm thấy dữ liệu tương ứng với id {id}" });
            }

            return Ok(res);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Features.New.Queries.GetAllNew;
using CleanArchitecture.Application.Features.New.Commands.CreateNew;
using CleanArchitecture.Application.Features.New.Commands.DeleteNew;
using CleanArchitecture.Application.Features.New.Queries.GetNewById;
using CleanArchitecture.Application.Features.New.Commands.UpdateNew;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await _mediator.Send( new GetAllNewQuery());
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _mediator.Send(new DeleteNewCommand { NewId = id });
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateNewCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _mediator.Send(new GetNewByIdQuery { NewId = id });
            return Ok(res);
        }
    }
}

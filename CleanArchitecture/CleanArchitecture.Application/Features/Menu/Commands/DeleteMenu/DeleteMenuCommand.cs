
using MediatR;

namespace CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu
{
    public class DeleteMenuCommand : IRequest<int>
    {
        public int MenuId { get; set; }
    }
}

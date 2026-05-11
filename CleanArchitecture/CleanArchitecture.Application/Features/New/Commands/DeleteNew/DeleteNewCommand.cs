
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.DeleteNew
{
    public class DeleteNewCommand : IRequest<int>
    {
        public int NewId { get; set; }
    }
}

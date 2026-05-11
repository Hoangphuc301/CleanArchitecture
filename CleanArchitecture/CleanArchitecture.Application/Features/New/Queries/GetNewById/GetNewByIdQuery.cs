
using CleanArchitecture.Application.Features.DTOs.New;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Queries.GetNewById
{
    public class GetNewByIdQuery : IRequest<NewDTO>
    {
        public int NewId { get; set; }
    }
}

using CleanArchitecture.Application.Features.DTOs.New;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Queries.GetAllNew
{
    public class GetAllNewQuery : IRequest<List<NewDTO>>
    {
    }
}

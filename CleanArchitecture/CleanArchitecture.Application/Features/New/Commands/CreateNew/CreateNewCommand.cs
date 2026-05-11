
using CleanArchitecture.Application.Features.DTOs.New;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.CreateNew
{
    public record CreateNewCommand
    (
        string Title,
        string? Slug,
        string? Summary,
        string? Content
    ) : IRequest<NewDTO>;
}

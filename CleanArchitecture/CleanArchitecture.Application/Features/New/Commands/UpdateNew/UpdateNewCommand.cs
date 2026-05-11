
using CleanArchitecture.Application.Features.DTOs.New;
using MediatR;

namespace CleanArchitecture.Application.Features.New.Commands.UpdateNew
{
    public record UpdateNewCommand
    (
        int NewId,
        string Title,
        string Slug,
        string Content
    ) : IRequest<NewDTO>;
}

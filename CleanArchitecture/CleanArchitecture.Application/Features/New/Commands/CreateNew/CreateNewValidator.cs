using CleanArchitecture.Application.Features.New.Commands.CreateNew;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Menu.Commands.CreateMenu
{
    public class CreateNewValidator : AbstractValidator<CreateNewCommand>
    {
        public CreateNewValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề là bắt buộc")
                .MaximumLength(100).WithMessage("Tiêu đề không được vượt quá 100 ký tự");

            RuleFor(x => x.Slug)
                .MaximumLength(500)
                .NotEmpty().WithMessage("Đường dẫn là bắt buộc")
                .WithMessage("Đường dẫn không được vượt quá 500 ký tự");

            RuleFor(x => x.Summary)
                .MaximumLength(500)
                .NotEmpty().WithMessage("Summary là bắt buộc")
                .WithMessage("Summary không được vượt quá 500 ký tự");

            RuleFor(x => x.Content)
                .MaximumLength(500)
                .NotEmpty().WithMessage("Nội dung là bắt buộc")
                .WithMessage("Nội dung không được vượt quá 500 ký tự");
        }
    }
}

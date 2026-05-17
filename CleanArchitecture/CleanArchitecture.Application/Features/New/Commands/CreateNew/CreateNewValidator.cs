using CleanArchitecture.Application.Features.New.Commands.CreateNew;
using FluentValidation;

namespace CleanArchitecture.Application.Features.New.Commands.CreateNew
{
    public class CreateNewValidator : AbstractValidator<CreateNewCommand>
    {
        public CreateNewValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề là bắt buộc")
                .MaximumLength(100).WithMessage("Tiêu đề không được vượt quá 100 ký tự");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Đường dẫn là bắt buộc")
                .MaximumLength(500).WithMessage("Đường dẫn không được vượt quá 500 ký tự");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Summary là bắt buộc")
                .MaximumLength(500).WithMessage("Summary không được vượt quá 500 ký tự");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Nội dung là bắt buộc")
                .MaximumLength(500).WithMessage("Nội dung không được vượt quá 500 ký tự");
        }
    }
}
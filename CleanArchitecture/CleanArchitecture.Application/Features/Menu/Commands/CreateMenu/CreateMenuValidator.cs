using FluentValidation;

namespace CleanArchitecture.Application.Features.Menu.Commands.CreateMenu
{
    public class CreateMenuValidator : AbstractValidator<CreateMenuCommand>
    {
        public CreateMenuValidator()
        {
            RuleFor(x => x.MenuName)
                .NotEmpty().WithMessage("Tên menu là bắt buộc")
                .MaximumLength(100).WithMessage("Tên menu không được vượt quá 100 ký tự");

            RuleFor(x => x.Slug)
                .MaximumLength(500)
                .WithMessage("Đường dẫn không được vượt quá 500 ký tự");

            RuleFor(x => x.DisplayOrder)
                .LessThanOrEqualTo(500)
                .WithMessage("DisplayOrder không được vượt quá 500 ký tự");
        }
    }
}

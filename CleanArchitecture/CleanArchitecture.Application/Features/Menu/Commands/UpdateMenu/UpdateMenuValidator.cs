using FluentValidation;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Application.Features.Menu.Commands.UpdateMenu
{
    public class UpdateMenuValidator : AbstractValidator<UpdateMenuCommand>
    {
        private readonly IMenuRepository _repository;

        public UpdateMenuValidator(IMenuRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.MenuId)
                .NotEmpty().WithMessage("ID menu là bắt buộc")
                .MustAsync(ExistInDatabase)
                .WithMessage("Menu không tồn tại");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên menu không được để trống")
                .MaximumLength(100)
                .WithMessage("Tên menu không quá 100 ký tự");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Thứ tự hiển thị không được là số âm");
        }

        private async Task<bool> ExistInDatabase( int id, CancellationToken cancellationToken)
        {
            var menu = await _repository.GetByIdAsync(id);

            return menu != null;
        }
    }
}
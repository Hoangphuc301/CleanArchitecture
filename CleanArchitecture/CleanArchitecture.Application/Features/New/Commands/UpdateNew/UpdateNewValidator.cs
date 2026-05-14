using CleanArchitecture.Application.Features.New.Commands.UpdateNew;
using CleanArchitecture.Domain.Interfaces;
using FluentValidation;

namespace CleanArchitecture.Application.Features.New.Commands.UpdateNew
{
    public class UpdateNewValidator : AbstractValidator<UpdateNewCommand>
    {
        private readonly INewRepository _repository;

        public UpdateNewValidator(INewRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.NewId)
                .NotEmpty().WithMessage("ID new là bắt buộc")
                .MustAsync(ExistInDatabase)
                .WithMessage("New không tồn tại");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống")
                .MaximumLength(500)
                .WithMessage("Tiêu đề không quá 500 ký tự");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Đường dẫn không được để trống")
                .MaximumLength(500)
                .WithMessage("Đường dẫn không quá 500 ký tự");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Summary không được để trống")
                .MaximumLength(100)
                .WithMessage("Summary không quá 100 ký tự");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Nội dung là bắt buộc");
        }

        private async Task<bool> ExistInDatabase(int id, CancellationToken cancellationToken)
        {
            var news = await _repository.GetByIdAsync(id);

            return news != null;
        }
    }
}

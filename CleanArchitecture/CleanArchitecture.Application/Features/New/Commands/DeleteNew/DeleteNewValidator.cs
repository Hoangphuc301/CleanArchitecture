using CleanArchitecture.Application.Features.New.Commands.DeleteNew;
using CleanArchitecture.Domain.Interfaces;
using FluentValidation;

namespace CleanArchitecture.Application.Features.New.Commands.DeleteNew
{
    public class DeleteNewValidator : AbstractValidator<DeleteNewCommand>
    {
        private readonly INewRepository _repository;

        public DeleteNewValidator(INewRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.NewId)
                .MustAsync(ExistInDatabase).WithMessage("New này không tồn tại trong hệ thống");
        }

        private async Task<bool> ExistInDatabase(int id, CancellationToken cancellationToken)
        {
            var menu = await _repository.GetByIdAsync(id);
            return menu != null;
        }
    }
}

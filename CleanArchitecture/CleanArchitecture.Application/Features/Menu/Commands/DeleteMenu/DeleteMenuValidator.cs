using CleanArchitecture.Application.Features.Menu.Commands.DeleteMenu;
using CleanArchitecture.Domain.Interfaces;
using FluentValidation;

public class DeleteMenuValidator : AbstractValidator<DeleteMenuCommand>
{
    private readonly IMenuRepository _repository;

    public DeleteMenuValidator(IMenuRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.MenuId)
            .MustAsync(ExistInDatabase).WithMessage("Menu này không tồn tại trong hệ thống");
    }

    private async Task<bool> ExistInDatabase(int id, CancellationToken cancellationToken)
    {
        var menu = await _repository.GetByIdAsync(id);
        return menu != null;
    }
}
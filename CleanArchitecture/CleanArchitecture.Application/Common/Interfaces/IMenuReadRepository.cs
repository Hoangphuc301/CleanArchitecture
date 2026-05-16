using CleanArchitecture.Application.Features.DTOs.Menu;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IMenuReadRepository
    {
        Task<List<MenuDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<MenuDto?> GetByIdAsync(int menuId, CancellationToken cancellationToken = default);
    }
}
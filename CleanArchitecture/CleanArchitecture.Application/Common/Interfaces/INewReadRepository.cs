
using CleanArchitecture.Application.Features.DTOs.New;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface INewReadRepository
    {
        Task<List<NewDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<NewDTO?> GetByIdAsync(int newId, CancellationToken cancellationToken = default);
    }
}

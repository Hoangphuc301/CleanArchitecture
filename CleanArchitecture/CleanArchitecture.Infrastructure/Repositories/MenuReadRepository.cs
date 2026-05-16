using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class MenuReadRepository : IMenuReadRepository
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public MenuReadRepository(MongoDbContext mongoDbContext, IMapper mapper)
        {
            _mongoDbContext = mongoDbContext;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var menuLogs = await _mongoDbContext.MenuLogs
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<MenuDto>>(menuLogs);
        }

        public async Task<MenuDto?> GetByIdAsync(int menuId, CancellationToken cancellationToken = default)
        {
            var menuLog = await _mongoDbContext.MenuLogs
                .Find(x => x.MenuId == menuId)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<MenuDto>(menuLog);
        }
    }
}
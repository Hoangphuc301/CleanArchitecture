using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.DTOs.Menu;
using CleanArchitecture.Application.Features.DTOs.New;
using CleanArchitecture.Infrastructure.Persistence.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class NewReadRepository : INewReadRepository
    {
        private readonly IMapper _mapper;
        private readonly MongoDbContext _mongoDbContext;

        public NewReadRepository(IMapper mapper, MongoDbContext mongoDbContext)
        {
            _mapper = mapper;
            _mongoDbContext = mongoDbContext;
        }

        public async Task<List<NewDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var newLogs = await _mongoDbContext.NewLogs
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<NewDTO>>(newLogs);
        }

        public async Task<NewDTO?> GetByIdAsync(int newId, CancellationToken cancellationToken = default)
        {
            var newLog = await _mongoDbContext.NewLogs
                .Find(x => x.NewId == newId)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<NewDTO>(newLog);
        }
    }
}

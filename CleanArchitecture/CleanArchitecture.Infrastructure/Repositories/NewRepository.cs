using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class NewRepository : INewRepository
    {
        private readonly ArchitectureDbContext _architectureDbContext;

        public NewRepository(ArchitectureDbContext architectureDbContext)
        {
            _architectureDbContext = architectureDbContext;
        }
        public async Task<News> CreateAsync(News news)
        {
            await _architectureDbContext.News.AddAsync(news);
            await _architectureDbContext.SaveChangesAsync();
            return news;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _architectureDbContext.News.
                Where(x => x.NewsId == id).ExecuteDeleteAsync();
        }

        public async Task<List<News>> GetAllAsync()
        {
            return await _architectureDbContext.News.ToListAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _architectureDbContext.News.
                Where(x => x.NewsId == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(int id, News news)
        {
            return await _architectureDbContext.News
               .Where(x => x.NewsId == id)
               .ExecuteUpdateAsync(x => x
                     .SetProperty(p => p.Title, news.Title)
                     .SetProperty(p => p.Slug, news.Slug)
                     .SetProperty(p => p.Summary, news.Summary)
                     .SetProperty(p => p.Content, news.Content)
                     .SetProperty(p => p.Thumbnail, news.Thumbnail)
                     .SetProperty(p => p.Author, news.Author)
                     .SetProperty(p => p.ViewCount, news.ViewCount)
                     .SetProperty(p => p.IsHot, news.IsHot)
                     .SetProperty(p => p.Status, news.Status)
                     .SetProperty(p => p.PublishDate, news.PublishDate)
                     .SetProperty(p => p.CreatedDate, news.CreatedDate)
                );
        }
    }
}

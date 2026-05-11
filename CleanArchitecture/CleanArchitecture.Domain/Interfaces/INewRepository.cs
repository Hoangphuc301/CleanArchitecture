using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface INewRepository
    {
        Task<List<News>> GetAllAsync();
        Task<News> GetByIdAsync(int id);
        Task<News> CreateAsync(News news);
        Task<int> UpdateAsync(int id, News news);
        Task<int> DeleteAsync(int id);
    }
}

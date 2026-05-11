using CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<Menus>> GetAllAsync();
        Task<Menus> GetByIdAsync(int id);
        Task<Menus> CreateAsync(Menus menu);
        Task<int> UpdateAsync (int id, Menus menu);
        Task<int> DeleteAsync(int id);
    }
}

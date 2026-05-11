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
    public class MenuRepository : IMenuRepository
    {
        private readonly ArchitectureDbContext _architectureDbContext;

        public MenuRepository(ArchitectureDbContext architectureDbContext)
        {
            _architectureDbContext = architectureDbContext;
        }
        public async Task<Menus> CreateAsync(Menus menu)
        {
            await _architectureDbContext.Menus.AddAsync(menu);
            await _architectureDbContext.SaveChangesAsync();
            return menu;
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _architectureDbContext.Menus.
                Where(x => x.MenuId == id).ExecuteDeleteAsync();
        }

        public async Task<List<Menus>> GetAllAsync()
        {
            return await _architectureDbContext.Menus.ToListAsync();
        }

        public async Task<Menus> GetByIdAsync(int id)
        {
            return await _architectureDbContext.Menus.
                Where(x => x.MenuId == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(int id, Menus menu)
        {
            return await _architectureDbContext.Menus
                .Where(x => x.MenuId == id)
                .ExecuteUpdateAsync(x => x
                      .SetProperty(p => p.MenuName, menu.MenuName)
                      .SetProperty(p => p.Slug, menu.Slug)
                      .SetProperty(p => p.Description, menu.Description)
                      .SetProperty(p => p.Icon, menu.Icon)
                      .SetProperty(p => p.DisplayOrder, menu.DisplayOrder)
                      .SetProperty(p => p.IsActive, menu.IsActive)
                      .SetProperty(p => p.CreatedDate, menu.CreatedDate) 
                 );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MovieManager.Interfaces;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Repositories
{
    public class CategoryRepository : ICategory
    {
        private readonly MovieManagerContext _context;

        public CategoryRepository(MovieManagerContext context)
        {
            _context = context;
        }

        public async Task<Category> Get(int categoryId)
        {
            return await _context.Categories.AsNoTracking().SingleOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _context.Categories.AsNoTracking().OrderBy(c=>c.Name).ToListAsync();
        }

        public async Task Add(Category category)
        {
            if (category != null)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(int categoryId, Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int categoryId)
        {
            var result = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);
            if(result != null)
            {
                _context.Categories.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

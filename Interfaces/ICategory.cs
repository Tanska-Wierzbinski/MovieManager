using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Interfaces
{
    public interface ICategory : IDisposable
    {
        Task<Category> Get(int categoryId);

        Task<IList<Category>> GetAll();

        Task Add(Category category);

        Task Update(int categoryId, Category category);

        Task Delete(int categoryId);
    }
}

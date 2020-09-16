using MovieManager.Models;
using MovieManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Interfaces
{
    public interface IMovie : IDisposable
    {
        Task<Movie> Get(int movieId);

        Task<IList<Movie>> GetAll();

        Task Add(MovieViewModel movie);

        Task Update(int movieId, MovieViewModel movie);

        Task Delete(int movieId);

        Task DeleteImage(string imageName);

        Task UploadImage(int movieId, Image image);

        Task<IList<Movie>> ListOfCategory(int categoryId);
    }
}

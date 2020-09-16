using MovieManager.Models;
using MovieManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Interfaces
{
    public interface IActor : IDisposable
    {
        Task<Actor> Get(int actorId);

        Task<IList<Actor>> GetAll();

        Task Add(ActorViewModel actor);

        Task Update(int actorId, ActorViewModel actor);

        Task Delete(int actorId);

        Task AddGrade(int movieId, int actorId, int grade);

        Task DeleteImage(string imageName);

        Task UploadImage(int actorId, Image image);

        Task<IList<Movie>> ListOfMovies(int actorId);
    }
}

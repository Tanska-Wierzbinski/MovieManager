using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Interfaces
{
    public interface IReview : IDisposable
    {
        Task<Review> Get(int reviewId);

        Task<IList<Review>> GetAll();

        Task Add(Review review);

        Task Update(int reviewId, Review review);

        Task Delete(int reviewId);
    }
}

using Microsoft.EntityFrameworkCore;
using MovieManager.Interfaces;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Repositories
{
    public class ReviewRepository : IReview
    {
        private readonly MovieManagerContext _context;

        public ReviewRepository(MovieManagerContext context)
        {
            _context = context;
        }

        public async Task<Review> Get(int reviewId)
        {
            return await _context.Reviews.Include(r=>r.Movie).SingleOrDefaultAsync(c => c.ReviewId == reviewId);
        }

        public async Task<IList<Review>> GetAll()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task Add(Review review)
        {
            if(review != null)
            {
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(int reviewId, Review review)
        {
            var result = await _context.Reviews.SingleOrDefaultAsync(r => r.ReviewId == reviewId);
            if(result!=null)
            {
                result.Description = review.Description;
                result.Grade = review.Grade;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int reviewId)
        {
            var result = await _context.Reviews.SingleOrDefaultAsync(c => c.ReviewId == reviewId);
            if (result != null)
            {
                _context.Reviews.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

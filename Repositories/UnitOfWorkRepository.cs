using Microsoft.AspNetCore.Hosting;
using MovieManager.Interfaces;
using MovieManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly MovieManagerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private ActorRepository _actorRepository;
        private MovieRepository _movieRepository;
        private CategoryRepository _categoryRepository;
        private ReviewRepository _reviewRepository;

        public UnitOfWorkRepository(MovieManagerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IMovie Movie
        {
            get
            {
                return _movieRepository = _movieRepository ?? new MovieRepository(_context, _hostEnvironment);
            }
        }

        public IActor Actor
        {
            get
            {
                return _actorRepository = _actorRepository ?? new ActorRepository(_context, _hostEnvironment);
            }
        }

        public ICategory Category
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);
            }
        }

        public IReview Review
        {
            get
            {
                return _reviewRepository = _reviewRepository ?? new ReviewRepository(_context);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

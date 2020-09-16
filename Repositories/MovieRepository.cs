using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieManager.Interfaces;
using MovieManager.Models;
using MovieManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Repositories
{
    public class MovieRepository : IMovie
    {
        private readonly MovieManagerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MovieRepository(MovieManagerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<Movie> Get(int movieId)
        {
            return await _context.Movies.AsNoTracking().Include(m => m.MovieCategories).ThenInclude(m => m.Category).Include(m => m.MovieActors).ThenInclude(m => m.Actor).ThenInclude(m=>m.Grades).Include(m => m.Reviews).SingleOrDefaultAsync(c => c.MovieId == movieId);
        }

        public async Task<IList<Movie>> GetAll()
        {
            return await _context.Movies.AsNoTracking().Include(m => m.Reviews).Include(m => m.MovieCategories).ThenInclude(m => m.Category).ToListAsync();
        }

        public async Task Add(MovieViewModel movie)
        {
            if (movie != null)
            {
                Movie newmovie = new Movie();
                newmovie.Name = movie.Name;
                newmovie.Director = movie.Director;
                newmovie.ReleaseDate = movie.ReleaseDate;
                await _context.Movies.AddAsync(newmovie);
                await _context.SaveChangesAsync();

                await UploadImage(newmovie.MovieId, movie.Image);

                if (movie.ActorIds != null)
                {
                    foreach (var item in movie.ActorIds)
                    {
                        MovieActor ma = new MovieActor();
                        ma.ActorId = item;
                        ma.MovieId = newmovie.MovieId;
                        await _context.MovieActors.AddAsync(ma);
                    }
                }

                if (movie.CategoryIds != null)
                {
                    foreach (var item in movie.CategoryIds)
                    {
                        MovieCategory mc = new MovieCategory();
                        mc.CategoryId = item;
                        mc.MovieId = newmovie.MovieId;
                        await _context.MovieCategories.AddAsync(mc);
                    }
                }

                if (movie.TempActors != null)
                {
                    foreach (var item in movie.TempActors)
                    {
                        Actor actor = new Actor();
                        actor.Name = item.Name;
                        actor.LastName = item.LastName;
                        actor.BornDate = item.BornDate;
                        if (item.DeathDate != null)
                            actor.DeathDate = item.DeathDate;
                        actor.Country = item.Country;
                        actor.Gender = item.Gender;
                        await _context.Actors.AddAsync(actor);
                        await _context.SaveChangesAsync();

                        await UploadActorImage(actor.ActorId, item.Image);

                        MovieActor ma = new MovieActor();
                        ma.ActorId = actor.ActorId;
                        ma.MovieId = newmovie.MovieId;
                        await _context.MovieActors.AddAsync(ma);

                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task Update(int movieId, MovieViewModel movie)
        {
            var result = await _context.Movies.SingleOrDefaultAsync(m => m.MovieId == movieId);

            if (result != null)
            {
                result.Name = movie.Name;
                result.Director = movie.Director;
                result.ReleaseDate = movie.ReleaseDate;

                await UploadImage(movieId, movie.Image);

                var res = await _context.MovieActors.Where(m => m.MovieId == movieId).ToListAsync();
                foreach (var item in res)
                {
                    _context.MovieActors.Remove(item);
                }

                var res2 = await _context.MovieCategories.Where(m => m.MovieId == movieId).ToListAsync();
                foreach (var item in res2)
                {
                    _context.MovieCategories.Remove(item);
                }

                if (movie.ActorIds != null)
                {
                    foreach (var item in movie.ActorIds)
                    {
                        MovieActor ma = new MovieActor();
                        ma.ActorId = item;
                        ma.MovieId = movieId;
                        await _context.MovieActors.AddAsync(ma);
                    }
                }

                if (movie.CategoryIds != null)
                {
                    foreach (var item in movie.CategoryIds)
                    {
                        MovieCategory mc = new MovieCategory();
                        mc.CategoryId = item;
                        mc.MovieId = movieId;
                        await _context.MovieCategories.AddAsync(mc);
                    }
                }

                if (movie.TempActors != null)
                {
                    foreach (var item in movie.TempActors)
                    {
                        Actor actor = new Actor();
                        actor.Name = item.Name;
                        actor.LastName = item.LastName;
                        await _context.Actors.AddAsync(actor);
                        await _context.SaveChangesAsync();

                        MovieActor ma = new MovieActor();
                        ma.ActorId = actor.ActorId;
                        ma.MovieId = movieId;
                        await _context.MovieActors.AddAsync(ma);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UploadActorImage(int actorId, Image image)
        {
            var result = await _context.Actors.SingleOrDefaultAsync(m => m.ActorId == actorId);

            if (image != null)
            {
                if (result != null)
                {
                    string wwwRothPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                    string extension = Path.GetExtension(image.ImageFile.FileName);

                    fileName = fileName + DateTime.Now.ToString("yyMMddss") + extension;
                    image.Name = fileName;

                    string path = Path.Combine(wwwRothPath + "/Image/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        image.ImageFile.CopyTo(fileStream);
                    }

                    if (await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName) != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", result.ImageName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        var img = await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName);
                        _context.Images.Remove(img);
                        await _context.SaveChangesAsync();
                    }
                    await _context.Images.AddAsync(image);
                    result.ImageName = image.Name;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete(int movieId)
        {
            var result = await _context.Movies.SingleOrDefaultAsync(c => c.MovieId == movieId);
            if (result != null)
            {
                var img = await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName);
                if (img != null)
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", img.Name);
                    if(System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _context.Images.Remove(img);
                }
                _context.Movies.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteImage(string imageName)
        {
            var result = await _context.Images.SingleOrDefaultAsync(c => c.Name == imageName);

            if (result != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", result.Name);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.Images.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UploadImage(int movieId, Image image)
        {
            var result = await _context.Movies.SingleOrDefaultAsync(m => m.MovieId == movieId);

            if (image != null)
            {
                if (result != null)  
                {
                    string wwwRothPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                    string extension = Path.GetExtension(image.ImageFile.FileName);

                    fileName = fileName + DateTime.Now.ToString("yyMMddss") + extension;
                    image.Name = fileName;

                    string path = Path.Combine(wwwRothPath + "/Image/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        image.ImageFile.CopyTo(fileStream);
                    }

                    if (await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName) != null)
                    {
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", result.ImageName);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        var img = await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName);
                        _context.Images.Remove(img);
                        await _context.SaveChangesAsync();
                    }
                    await _context.Images.AddAsync(image);
                    result.ImageName = image.Name;
                    await _context.SaveChangesAsync();
                }
            }
        }


        public async Task<IList<Movie>> ListOfCategory(int categoryId)
        {
            IList<Category> result = await _context.Categories.Include(m => m.MovieCategories).ThenInclude(m => m.Movie).ThenInclude(m=>m.Reviews).ToListAsync();
            Category category = result.Where(m => m.CategoryId == categoryId).Single();
            IList<Movie> movies = category.MovieCategories.Select(s => s.Movie).ToList();

            return movies;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using MovieManager.Interfaces;
using MovieManager.Models;
using MovieManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MovieManager.Repositories
{
    public class ActorRepository : IActor
    {
        private readonly MovieManagerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ActorRepository(MovieManagerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

         public async Task<Actor> Get(int actorId)
        {
            return await _context.Actors.AsNoTracking().Include(m => m.MovieActors).ThenInclude(m => m.Movie).Include(m=>m.Grades).SingleOrDefaultAsync(a => a.ActorId == actorId);
        }

        public async Task<IList<Actor>> GetAll()
        {
            return await _context.Actors.AsNoTracking().OrderBy(a=>a.LastName).Include(a=>a.Grades).ToListAsync();
        }

        public async Task Add(ActorViewModel actor)
        {
            if (actor != null)
            {
                Actor newactor = new Actor();
                newactor.Name = actor.Name;
                newactor.LastName = actor.LastName;
                newactor.BornDate = actor.BornDate;
                await _context.Actors.AddAsync(newactor);
                await _context.SaveChangesAsync();

                await UploadImage(newactor.ActorId, actor.Image);

                if (actor.MovieIds != null)
                {
                    foreach (var item in actor.MovieIds)
                    {
                        MovieActor ma = new MovieActor();
                        ma.ActorId = newactor.ActorId;
                        ma.MovieId = item;
                        await _context.MovieActors.AddAsync(ma);
                    }
                }

                if (actor.TempMovies != null)
                {
                    foreach (var item in actor.TempMovies)
                    {
                        Movie movie = new Movie();
                        movie.Name = item.Name;
                        movie.Director = item.Director;
                        movie.ReleaseDate = item.ReleaseDate;
                        await _context.Movies.AddAsync(movie);
                        await _context.SaveChangesAsync();

                        if (item.CategoryIds != null)
                        {
                            foreach (var cat in item.CategoryIds)
                            {
                                MovieCategory mc = new MovieCategory();
                                mc.CategoryId = cat;
                                mc.MovieId = movie.MovieId;
                                await _context.MovieCategories.AddAsync(mc);
                            }
                        }

                        MovieActor ma = new MovieActor();
                        ma.ActorId = newactor.ActorId;
                        ma.MovieId = movie.MovieId;
                        await _context.MovieActors.AddAsync(ma);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(int actorId, ActorViewModel actor)
        {
            var result = await _context.Actors.SingleOrDefaultAsync(x => x.ActorId == actorId);
            if (result != null)
            {
                result.Name = actor.Name;
                result.LastName = actor.LastName;
                result.Gender = actor.Gender;
                result.BornDate = actor.BornDate;
                if(actor.DeathDate!=null)
                    result.DeathDate = actor.DeathDate;
                
                result.Country = actor.Country;

                await UploadImage(actorId, actor.Image);

                var res = await _context.MovieActors.Where(m => m.ActorId == actorId).ToListAsync();
                foreach (var item in res)
                {
                    _context.MovieActors.Remove(item);
                }

                if (actor.MovieIds != null)
                {
                    foreach (var item in actor.MovieIds)
                    {
                        {
                            var ma = new MovieActor();
                            ma.ActorId = actorId;
                            ma.MovieId = item;
                            await _context.MovieActors.AddAsync(ma);
                        }
                    }
                }

                if (actor.TempMovies != null)
                {
                    foreach (var item in actor.TempMovies)
                    {
                        Movie movie = new Movie();
                        movie.Name = item.Name;
                        movie.Director = item.Director;
                        movie.ReleaseDate = item.ReleaseDate;

                        await _context.Movies.AddAsync(movie);
                        await _context.SaveChangesAsync();

                        if (item.CategoryIds != null)
                        {
                            foreach (var cat in item.CategoryIds)
                            {
                                MovieCategory mc = new MovieCategory();
                                mc.CategoryId = cat;
                                mc.MovieId = movie.MovieId;
                                await _context.MovieCategories.AddAsync(mc);
                            }
                        }
                        MovieActor ma = new MovieActor();
                        ma.ActorId = actorId;
                        ma.MovieId = movie.MovieId;

                        await _context.MovieActors.AddAsync(ma);
                        await _context.SaveChangesAsync();
                    }
                }
                await _context.SaveChangesAsync();
            }

        }

        public async Task Delete(int actorId)
        {
            var result = await _context.Actors.SingleOrDefaultAsync(a => a.ActorId == actorId);
            if (result != null)
            {
                var img = await _context.Images.SingleOrDefaultAsync(i => i.Name == result.ImageName);
                if (img != null)
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", img.Name);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    _context.Images.Remove(img);
                }
                _context.Actors.Remove(result);
               await _context.SaveChangesAsync();
            }
        }

        public async Task AddGrade(int movieId, int actorId, int grade)
        {
            if(await _context.MovieActors.SingleOrDefaultAsync(m=>m.MovieId == movieId && m.ActorId == actorId) != null)
            {
                var actorGrade = new Grade();
                actorGrade.ActorId = actorId;
                actorGrade.MovieId = movieId;
                actorGrade.GradeValue = grade;

                await _context.Grades.AddAsync(actorGrade);
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

        public async Task UploadImage(int actorId, Image image)
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


        public async Task<IList<Movie>> ListOfMovies(int actorId)
        {
            IList<Actor> result = await _context.Actors.Include(m => m.MovieActors).ThenInclude(m => m.Movie).ToListAsync();
            Actor actor = result.Where(m => m.ActorId == actorId).Single();
            IList<Movie> movies = actor.MovieActors.Select(s => s.Movie).ToList();

            return movies;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

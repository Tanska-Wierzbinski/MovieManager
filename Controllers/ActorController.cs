using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using MovieManager.Interfaces;
using MovieManager.Models;
using MovieManager.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace MovieManager.Controllers
{
    public class ActorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Actor
        public async Task<IActionResult> Index(Gender? gender, int yearMin, int yearMax, int gradeMin, int gradeMax, string[] countries, string sortOrder, int? pageNumber, int pageSize = 5)
        {
            var vm = new ActorIndexViewModel
            {
                Actors = await _unitOfWork.Actor.GetAll()
            };

            if (countries.Length != 0)
            {
                foreach (var country in countries)
                {
                    IList<Actor> result = await _unitOfWork.Actor.GetAll();
                    vm.Actors = vm.Actors.Intersect(result.Where(a => a.Country == country));
                }
            }

            vm.Filter.GradeMin = gradeMin;
            if (gradeMax == 0) vm.Filter.GradeMax = gradeMin;
            else vm.Filter.GradeMax = gradeMax;
            vm.Filter.YearMax = yearMax;
            if (yearMin == 0) vm.Filter.YearMin = yearMax;
            else vm.Filter.YearMin = yearMin;
            vm.Filter.Countries = countries;

            if(gender != null)
            {
                vm.Filter.Gender = gender.Value;
                vm.Actors = vm.Actors.Where(m => m.Gender == vm.Filter.Gender);
            }

            if (yearMin != 0 && yearMax != 0)
            {
                vm.Actors = vm.Actors.Where(m => m.BornDate.Year >= vm.Filter.YearMin && m.BornDate.Year <= vm.Filter.YearMax);
            }

            if (gradeMin != 0 && gradeMax != 0)
            {
                vm.Actors = vm.Actors.Where(m => m.Grades.Any())
                                        .Where(m => m.Grades
                                        .Average(m => m.GradeValue) >= vm.Filter.GradeMin && m.Grades
                                        .Average(m => m.GradeValue) < vm.Filter.GradeMax + 1);
            }

            switch (sortOrder)
            {
                case "nameDesc":
                    vm.Actors = vm.Actors.OrderByDescending(m => m.LastName);
                    break;
                case "bornDesc":
                    vm.Actors = vm.Actors.OrderByDescending(m => m.BornDate);
                    break;
                case "born":
                    vm.Actors = vm.Actors.OrderBy(m => m.BornDate);
                    break;
                case "gradeDesc":
                    vm.Actors = vm.Actors.Where(m => m.Grades.Any()).OrderByDescending(m => m.Grades.Average(k => k.GradeValue)).Union(vm.Actors);
                    break;
                case "grade":
                    vm.Actors = vm.Actors.Where(m => m.Grades.Any()).OrderBy(m => m.Grades.Average(k => k.GradeValue)).Union(vm.Actors);
                    break;
                case "quantityGradeDesc":
                    vm.Actors = vm.Actors.Where(m => m.Grades.Any()).OrderByDescending(m => m.Grades.Count()).Union(vm.Actors);
                    break;
                case "quantityGrade":
                    vm.Actors = vm.Actors.Where(m => m.Grades.Any()).OrderBy(m => m.Grades.Count()).Union(vm.Actors);
                    break;
                default:
                    vm.Actors = vm.Actors.OrderBy(m => m.LastName);
                    break;
            }

            vm.PaginatedActors = PaginatedList<Actor>.Create(vm.Actors.AsQueryable(), pageNumber ?? 1, pageSize);
            return View(vm);
        }

        // GET: Actor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _unitOfWork.Actor.Get(id);
            var vm = new ActorViewModel
            {
                ActorId = result.ActorId,
                Name = result.Name,
                LastName = result.LastName,
                Gender = result.Gender,
                BornDate = result.BornDate,
                Grades = result.Grades,
                MovieActors = result.MovieActors,
                ImageName = result.ImageName,
                Country = result.Country,
                Image = new Image
                {
                    Name = result.ImageName
                }
            };
            if (vm.DeathDate == null)
            {
                DateTime now = DateTime.Now;

                vm.Age = now.Year - vm.BornDate.Year; if (vm.BornDate.Date > now.AddYears(-vm.Age)) { vm.Age--; }
            }
            else
            {
                vm.Age = vm.DeathDate.Value.Year - vm.BornDate.Year; if (vm.BornDate.Date > vm.DeathDate.Value.AddYears(-vm.Age)) { vm.Age--; }
            }
            return View(vm);
        }


        //GET: Actor/Create
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> Create()
        {
            var vm = new ActorViewModel();
            IList<Movie> result = await _unitOfWork.Movie.GetAll();
            vm.Movies = result.Select(m => new SelectListItem { Text = m.Name, Value = m.MovieId.ToString() }).ToList();
            return View(vm);
        }

        // POST: Actor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Create(ActorViewModel actor)
        {
            await _unitOfWork.Actor.Add(actor);
            return RedirectToAction(nameof(Index));
        }

        // GET: Actor/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            //IList<Movie> movies = new List<Movie>();
            //movies = _unitOfWork.Movie.GetAll();
            //ViewBag.Movies = movies;
            var result = await _unitOfWork.Actor.Get(id);
            var vm = new ActorViewModel();
            var movie_result = await _unitOfWork.Movie.GetAll();
            vm.Movies = movie_result.Select(m => new SelectListItem { Text = m.Name, Value = m.MovieId.ToString() }).ToList();

            


            vm.ActorId = result.ActorId;
            vm.Name = result.Name;
            vm.LastName = result.LastName;
            vm.BornDate = result.BornDate;
            vm.DeathDate = result.DeathDate;
            vm.Gender = result.Gender;
            vm.Country = result.Country;
            vm.Image = new Image();
           // vm.Image.ImageId = result.ActorId;
            vm.Image.Name = result.ImageName;

            vm.MovieIds = new int[result.MovieActors.Count()];
            int count = 0;
            foreach (var item in result.MovieActors)
            {
                vm.MovieIds[count] = item.MovieId;
                count++;
            }
            //ViewData["movies"] = _unitOfWork.Movie.GetAll().Select(m => new SelectListItem { Text = m.Name, Value = m.MovieId.ToString() }).ToList();
            return View(vm);
        }

        // POST: Actor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Edit(int id, ActorViewModel actor)
        {
            await _unitOfWork.Actor.Update(id, actor);
            return RedirectToAction(nameof(Index));
        }

        // GET: Actor/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(int id)
        {

            return View(await _unitOfWork.Actor.Get(id));
        }

        // POST: Actor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(int id, Actor actor)
        {
            await _unitOfWork.Actor.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> DeleteImage(string imageName)
        {
            await _unitOfWork.Actor.DeleteImage(imageName);

            return Ok();
        }

        [Authorize]
        public async Task<IActionResult> AddGrade(int movieId, int actorId, int grade)
        {
            await _unitOfWork.Actor.AddGrade(movieId, actorId, grade);

            return RedirectToAction(nameof(Details), new { id= actorId});
        }
        [Authorize]
        public async Task<IActionResult> AddGradeInMovie(int movieId, int actorId, int grade)
        {
            await _unitOfWork.Actor.AddGrade(movieId, actorId, grade);

            return RedirectToAction(nameof(Details), nameof(Movie), new { id = movieId });
        }

        public async Task<IActionResult> ListOfMovies(int id) //id => actor id
        {
            var ac = await _unitOfWork.Actor.Get(id);
            ViewBag.actor = ac.Name + " " + ac.LastName;
            ViewBag.id = id;
            return View(_unitOfWork.Actor.ListOfMovies(id));
        }
    }
}
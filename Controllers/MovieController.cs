using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieManager.Interfaces;
using MovieManager.Models;
using MovieManager.ViewModels;

namespace MovieManager.Controllers
{
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: MovieController
        public async Task<ActionResult> Index(string sortOrder, int yearMin, int yearMax, int gradeMin, int gradeMax, int[] categories, int? pageNumber, int pageSize=5)
        {
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewData["CurrentSort"] = sortOrder;
            var vm = new MovieIndexViewModel();
            vm.Categories = await _unitOfWork.Category.GetAll();

            if (categories.Length != 0)
            {
                vm.Filter.Categories = categories;
                foreach (var item in categories)
                {
                    if (vm.Movies != null)
                    {
                        vm.Movies = vm.Movies.Intersect(await _unitOfWork.Movie.ListOfCategory(item));
                    }
                    else
                    {
                        vm.Movies = await _unitOfWork.Movie.ListOfCategory(item);
                    }
                }
            }
            else
            {
                vm.Movies = await _unitOfWork.Movie.GetAll(); 
            }

            vm.Filter.GradeMin = gradeMin;
            if(gradeMax == 0) vm.Filter.GradeMax = gradeMin;
            else vm.Filter.GradeMax = gradeMax;
            vm.Filter.YearMax = yearMax;
            if (yearMin == 0) vm.Filter.YearMin = yearMax;
            else vm.Filter.YearMin = yearMin;

            if (yearMin != 0 && yearMax != 0)
            {
                vm.Movies = vm.Movies.Where(m => m.ReleaseDate.Year >= vm.Filter.YearMin && m.ReleaseDate.Year <= vm.Filter.YearMax);
            }

            if (gradeMin != 0 && gradeMax != 0)
            {
                vm.Movies = vm.Movies.Where(m => m.Reviews.Any()).Where(m => m.Reviews.Average(m => m.Grade) >= vm.Filter.GradeMin && m.Reviews.Average(m => m.Grade) < vm.Filter.GradeMax+1);
         
            }
            
            switch (sortOrder)
            {
                case "nameDesc":
                    vm.Movies = vm.Movies.OrderByDescending(m => m.Name);
                    break;
                case "yearDesc":
                    vm.Movies = vm.Movies.OrderByDescending(m => m.ReleaseDate);
                    break;
                case "year":
                    vm.Movies = vm.Movies.OrderBy(m => m.ReleaseDate);
                    break;
                case "gradeDesc":
                    vm.Movies = vm.Movies.Where(m => m.Reviews.Any()).OrderByDescending(m => m.Reviews.Average(k => k.Grade)).Union(vm.Movies);
                    break;
                case "grade":
                    vm.Movies = vm.Movies.Where(m => m.Reviews.Any()).OrderBy(m => m.Reviews.Average(k => k.Grade)).Union(vm.Movies);
                    break;
                case "quantityGradeDesc":
                    vm.Movies = vm.Movies.Where(m => m.Reviews.Any()).OrderByDescending(m => m.Reviews.Count()).Union(vm.Movies);
                    break;
                case "quantityGrade":
                    vm.Movies = vm.Movies.Where(m => m.Reviews.Any()).OrderBy(m => m.Reviews.Count()).Union(vm.Movies);
                    break;
                default:
                    vm.Movies = vm.Movies.OrderBy(m => m.Name);
                    break;
            }
            vm.PaginatedMovies = PaginatedList<Movie>.Create(vm.Movies.AsQueryable(), pageNumber ?? 1, pageSize);
            return View(vm);
        }

        // GET: MovieController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = await _unitOfWork.Movie.Get(id);

            MovieDetailsViewModel vm = new MovieDetailsViewModel();
            vm.MovieId = result.MovieId;
            vm.Name = result.Name;
            vm.Director = result.Director;
            vm.ReleaseDate = result.ReleaseDate;
            vm.ImageName = result.ImageName;
            vm.Reviews = result.Reviews;
            vm.MovieActors = result.MovieActors;
            vm.MovieCategories = result.MovieCategories;
            vm.Review = new Review();
            vm.Review.MovieId = result.MovieId;
            vm.SelectListGrade = new List<SelectListItem>
             {
                 new SelectListItem {Text = "1", Value = "1"},
                 new SelectListItem {Text = "2", Value = "2"},
                 new SelectListItem {Text = "3", Value = "3"},
                 new SelectListItem {Text = "4", Value = "4"},
                 new SelectListItem {Text = "5", Value = "5"},
                 new SelectListItem {Text = "6", Value = "6"},
                 new SelectListItem {Text = "7", Value = "7"},
                 new SelectListItem {Text = "8", Value = "8"},
                 new SelectListItem {Text = "9", Value = "9"},
                 new SelectListItem {Text = "10", Value = "10"}
            };
            return View(vm);
        }

        // GET: MovieController/Create
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create()
        {
            MovieViewModel mv = new MovieViewModel();
            var actor_result = await _unitOfWork.Actor.GetAll();
            mv.Actors = actor_result.Select(m => new SelectListItem { Text = (m.Name + " " + m.LastName), Value = m.ActorId.ToString() }).ToList();
            var category_result = await _unitOfWork.Category.GetAll();
            mv.Categories = category_result.Select(m => new SelectListItem { Text = m.Name, Value = m.CategoryId.ToString() }).ToList();
            return View(mv);
        }

        // POST: MovieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create(MovieViewModel movie)
        {
            await _unitOfWork.Movie.Add(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: MovieController/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _unitOfWork.Movie.Get(id);

            MovieViewModel vm = new MovieViewModel();
            vm.Id = result.MovieId;
            vm.Name = result.Name;
            vm.Director = result.Director;
            vm.ReleaseDate = result.ReleaseDate;
            vm.Image = new Image();
            vm.Image.Name = result.ImageName;

            var actor_result = await _unitOfWork.Actor.GetAll();
            var category_result = await _unitOfWork.Category.GetAll();

            vm.Actors = actor_result.Select(m => new SelectListItem { Text = (m.Name + " " + m.LastName), Value = m.ActorId.ToString() }).ToList();
            vm.Categories = category_result.Select(m => new SelectListItem { Text = m.Name, Value = m.CategoryId.ToString() }).ToList();

            vm.CategoryIds = new int[result.MovieCategories.Count()];
            vm.ActorIds = new int[result.MovieActors.Count()];

            int count = 0;
            foreach (var item in result.MovieCategories)
            {
                vm.CategoryIds[count] = item.CategoryId;
                count++;
            }

            count = 0;

            foreach (var item in result.MovieActors)
            {
                vm.ActorIds[count] = item.ActorId;
                count++;
            }
            return View(vm);
        }

        // POST: MovieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int id, MovieViewModel movie)
        {
            await _unitOfWork.Movie.Update(id, movie);
            return RedirectToAction(nameof(Index), nameof(Home));
        }

        // GET: MovieController/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Movie.Get(id));
        }

        // POST: MovieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int id, Movie movie)
        {
            await _unitOfWork.Movie.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteImage(string imageName)
        {
            await _unitOfWork.Movie.DeleteImage(imageName);

            return Ok();
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> UploadImage(int movieId, Image image)
        {
            await _unitOfWork.Movie.UploadImage(movieId, image);
            return PartialView("~/Views/Shared/EditorTemplates/ImageMovie.cshtml", image);
        }

        public async Task<ActionResult> ListOfCategory(int id) //id => category id
        {
            var mv = await _unitOfWork.Category.Get(id);
            ViewBag.category = mv.Name;
            ViewBag.id = id;
            return View(await _unitOfWork.Movie.ListOfCategory(id));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult AddNewActor()
        {
            var tempActor = new TempActor();
            return PartialView("~/Views/Shared/EditorTemplates/TempActor.cshtml", tempActor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Interfaces;
using MovieManager.ViewModels;

namespace MovieManager.Controllers
{
    public class Home : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public Home(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.Movie.GetAll();

            var moviesWithReviews = result.Where(m => m.Reviews.Any());

            HomeViewModel home = new HomeViewModel();
            home.TopMovies = moviesWithReviews.OrderByDescending(m => m.Reviews.Average(m => m.Grade)).Take(3).ToList();
            home.NewMovies = result.OrderByDescending(m => m.ReleaseDate).Take(3).ToList();
            return View(home);
        }
        public async Task<IActionResult> SearchIndex(string searchString)
        {
            var vm = new SearchIndexViewModel();
            vm.Movies = await _unitOfWork.Movie.GetAll();
            vm.Actors = await _unitOfWork.Actor.GetAll();
            ViewBag.searchString = searchString;
            if (searchString != null)
            {
                vm.Movies = vm.Movies.Where(m => m.Name.ToLower().Contains(searchString.ToLower()));
                vm.Actors = vm.Actors.Where(m => m.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(vm);
        }
    }
}

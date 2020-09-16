using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Interfaces;
using MovieManager.Models;

namespace MovieManager.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ReviewController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: ReviewController
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Review.GetAll());
        }

        // GET: ReviewController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _unitOfWork.Review.Get(id));
        }

        // GET: ReviewController/Create
        [Authorize]
        public async Task<ActionResult> Create(int id) //id => movieId
        {
            ViewBag.mID = id;
            return View(new Review());
        }

        // POST: ReviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Review review, int id) //id => movieId
        {
            if (_signInManager.IsSignedIn(User))
                review.Author = User.Identity.Name;
            review.MovieId = id;
            await _unitOfWork.Review.Add(review);
            return RedirectToAction(nameof(Details), nameof(Movie), new { id = review.MovieId });
        }

        // GET: ReviewController/Edit/5
        [Authorize(Policy = "EditReview")]
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _unitOfWork.Review.Get(id));
        }

        // POST: ReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditReview")]
        public async Task<ActionResult> Edit(int id, Review review)
        {
            await _unitOfWork.Review.Update(id, review);
            return RedirectToAction(nameof(Details), nameof(Movie), new { id = (await _unitOfWork.Review.Get(id)).MovieId });
        }

        // GET: ReviewController/Delete/5
        [Authorize(Policy = "EditReview")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Review.Get(id));
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditReview")]
        public async Task<ActionResult> Delete(int id, int movieId)
        {
            int mId = (await _unitOfWork.Review.Get(id)).MovieId;
            await _unitOfWork.Review.Delete(id);
            return RedirectToAction(nameof(Details), nameof(Movie), new { id = mId });
        }
    }
}

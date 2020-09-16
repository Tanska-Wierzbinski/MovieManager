using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Interfaces;
using MovieManager.Models;

namespace MovieManager.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Category.GetAll());
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _unitOfWork.Category.Get(id));
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
        {
            return View(new Category());
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            await _unitOfWork.Category.Add(category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _unitOfWork.Category.Get(id));
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            await _unitOfWork.Category.Update(id, category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _unitOfWork.Category.Get(id));
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Category category)
        {
            await _unitOfWork.Category.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

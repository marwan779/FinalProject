using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Owner/Category
        public IActionResult Index()
        {
            try
            {
                var categories = _unitOfWork.CategoryRepository.GetAllCategories();
                return View(categories);
            }
            catch (Exception)
            {
                TempData["error"] = "Failed to load categories.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Owner/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owner/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _unitOfWork.CategoryRepository.Add(model);
                _unitOfWork.Save();

                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = "Failed to create category.";
                return View(model);
            }
        }

        // GET: Owner/Category/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);
                if (category == null)
                {
                    TempData["error"] = "Category not found!";
                    return RedirectToAction("Index");
                }

                return View(category);
            }
            catch (Exception)
            {
                TempData["error"] = "Failed to load category.";
                return RedirectToAction("Index");
            }
        }

        // POST: Owner/Category/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                _unitOfWork.CategoryRepository.Update(model);
                _unitOfWork.Save();

                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = "Failed to update category.";
                return View(model);
            }
        }

        public IActionResult Delete(int CategoryId)
        {
            try
            {
                var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == CategoryId);
                if (category == null)
                {
                    TempData["error"] = "Category not found.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.CategoryRepository.Remove(category);
                _unitOfWork.Save();

                TempData["success"] = "Category deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = "Failed to delete category.";
                return RedirectToAction("Index");
            }
        }
    }
}

using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;
using InventoryManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = StaticDetails.OwnerRole)]
    public class OwnerController : Controller
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerController(IApplicationUserRepository userRepo, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        // ---------------- Index (List Suppliers) ----------------
        public async Task<IActionResult> Index()
        {
            var allUsers = _userRepo.GetAll().ToList();
            var supplierViewModels = new List<UserViewModel>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(StaticDetails.SupplierRole))
                {
                    supplierViewModels.Add(new UserViewModel
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Role = string.Join(", ", roles),
                        CreatedAt = user.CreatedAt
                    });
                }
            }

            return View(supplierViewModels);
        }

        // ---------------- Create Supplier ----------------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model, string password)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Email.ToUpper(),
                PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "0000000000" : model.PhoneNumber,
                Address = string.IsNullOrEmpty(model.Address) ? "No Address Provided" : model.Address,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, StaticDetails.SupplierRole);
                TempData["Success"] = "Supplier created successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ---------------- GET Edit Supplier ----------------
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }

        // ---------------- POST Edit Supplier ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.NormalizedUserName = model.Email.ToUpper();
            user.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "0000000000" : model.PhoneNumber;
            user.Address = string.IsNullOrEmpty(model.Address) ? "No Address Provided" : model.Address;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Supplier updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ---------------- Delete Supplier ----------------
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "Supplier deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
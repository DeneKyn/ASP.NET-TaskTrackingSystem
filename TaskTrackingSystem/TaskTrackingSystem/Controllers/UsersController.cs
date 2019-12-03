using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.ViewModels;

namespace TaskTrackingSystem.Controllers
{
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            List<UsersListViewModel> model = new List<UsersListViewModel>();
            foreach (ApplicationUser user in users)
            {
                if (user != null)
                {
                    // получем список ролей пользователя
                    var userRoles = await _userManager.GetRolesAsync(user);
                    model.Add(new UsersListViewModel
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        UserRoles = userRoles
                    });
                }                
            }
            return View(model);            
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {                
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                EditUserViewModel model = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;                    
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var allRoles = _roleManager.Roles.ToList();                    
                    var addedRoles = roles.Except(userRoles);
                    var removedRoles = userRoles.Except(roles);

                    await _userManager.AddToRolesAsync(user, addedRoles);

                    await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskTrackingSystem.ViewModels;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class ProjectsController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;        
        private ApplicationContext db;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public ProjectsController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            db = context;
        }

        public async Task<ActionResult> Index(string username)
        {
            ApplicationUser user = new ApplicationUser();
            if (username == null)            
                username = User.Identity.Name;                
            
            user = await _userManager.FindByNameAsync(username);
            var projects = db.Projects.Where(p => p.UserId == user.Id);
            ProjectViewModel model = new ProjectViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.UserName,                
                Projects = projects.ToList()

            };
            return View(model);
            
        }

        public ActionResult Create()
        {            
            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
                return PartialView(project);


            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            project.UserId = user.Id;      
            db.Projects.Add(project);            
            db.SaveChanges();            
            return Json(new { success = true });     
        }
    }
}

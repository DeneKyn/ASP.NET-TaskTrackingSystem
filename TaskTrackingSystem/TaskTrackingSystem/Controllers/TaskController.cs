using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.ViewModels;
using TaskTrackingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskTrackingSystem.Controllers
{

    public class TaskController : Controller
    {
        private ApplicationContext db;
        UserManager<ApplicationUser> _userManager;
        private int CurrentTaskListId;
        private IProjectService _project;
        private ITaskListService _tasklist;

        public TaskController(ApplicationContext context, UserManager<ApplicationUser> userManager, IProjectService project, ITaskListService tasklist)
        {
            db = context;
            _userManager = userManager;
            CurrentTaskListId = -1;
            _project = project;
            _tasklist = tasklist;

        }

        
        public IActionResult Index(int id)
        {
            var kek = db.ProjectTasks.FirstOrDefault(p => p.Id == id);
            
            return View(kek);
        }
        
        public async Task<ActionResult> Create(int id)
        {
            
            ViewBag.TaskListId = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectTask task, int id)
        {
            if (!ModelState.IsValid)
                return PartialView(task);

            ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            task.TaskList = db.TaskLists.FirstOrDefault(p => p.Id == id);
            
            db.ProjectTasks.Add(new ProjectTask { Name = task.Name, Description=task.Description, Author = user, TaskList = task.TaskList });
            db.SaveChanges();
            return Json(new { success = true });
        }

        [Authorize]
        public async Task<ActionResult> Change(int id)
        {
            
            var kek = db.ProjectTasks.FirstOrDefault(p => p.Id == id);
            var lol = _tasklist.GetById(kek.TaskListId);
            var lolkek = _project.GetAllById(lol.ProjectId);
            
            //return Json(lolkek);
            ViewBag.TaskLists = new SelectList(lolkek.TaskLists, "Id", "Name");
            var model = new ProjectTaskViewModel{ CurrentTask = kek};
            //return Json(model);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Change(int id, ProjectTaskViewModel model)
        {
            return Json(new { currentid = id, changeid = model.ChangeId});
            
        }
    }
}

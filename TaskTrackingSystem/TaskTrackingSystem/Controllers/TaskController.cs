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
        private IProjectService _project;
        private ITaskListService _tasklist;
        private ITaskService _task;
        private IApplicationUser _user;
        public TaskController(IProjectService project, ITaskListService tasklist, ITaskService task, IApplicationUser user)
        {
                      
            _project = project;
            _tasklist = tasklist;
            _user = user;
            _task = task;

        }


        public IActionResult Index(int id)
        {
            return View(_task.GetById(id));
        }

        public ActionResult Create(int id)
        {
            ViewBag.TaskListId = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectTask task, int id)
        {
            if (ModelState.IsValid)
            {
                await _task.Create(task, id);
                return Json(new { success = true });
            }
            return PartialView(task);
        }

        [Authorize]
        public async Task<ActionResult> Change(int id)
        {
            Project project = _task.GetProject(id);
            ViewBag.TaskLists = new SelectList(project.TaskLists, "Id", "Name");
            var model = new ProjectTaskViewModel { CurrentTask = _task.GetById(id), TaskLists = project.TaskLists.ToList() };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Change(int id, ProjectTaskViewModel model)
        {
            await _task.ChangePosition(id, model.ChangeId);            
            return Json(new { success = true });
        }
    }
}

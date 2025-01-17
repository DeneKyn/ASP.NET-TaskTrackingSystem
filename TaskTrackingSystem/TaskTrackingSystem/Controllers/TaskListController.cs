﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Services;
using TaskTrackingSystem.ViewModels;

namespace TaskTrackingSystem.Controllers
{
    
    public class TaskListController : Controller
    {

        
        private IProjectService _project;
        private ITaskListService _tasklist;
        private IApplicationUser _user;
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext db;
        public TaskListController(IProjectService project, ITaskListService tasklist, IApplicationUser user, UserManager<ApplicationUser> userManager, ApplicationContext _context)
        {
            _project = project;
            _tasklist = tasklist;
            _user = user;
            _userManager = userManager;
            db = _context;
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            ApplicationUser user = _user.Get();
            Project project = _project.GetById(id);
            
            if (_project.CheckOwner(project.UserId) | _project.CheckTeam(project.Id))
            {
                ViewBag.ProjectId = id;
                return PartialView();
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(TaskList taskList, int id)
        {
            if (ModelState.IsValid)
            {
                await _tasklist.Create(taskList, id);
                return Json(new { success = true });
            }
            return PartialView(taskList);
        }

        public IActionResult Index(int id, string name)
        {

            ApplicationUser user = _user.Get(name);            
            var project = _project.Get(name).ToList()[id];
            var tasklists = _tasklist.GetFull(project.Id);


            TaskListViewModel model = new TaskListViewModel
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                TaskLists = tasklists,
                isOwner = _project.CheckOwner(project.UserId),
                isTeam = _project.CheckTeam(project.Id),
                Team = project.UserProjects,
                AllUser = _user.GetAll()
            };
            
            return View(model);

        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            TaskList taskList = _tasklist.GetById(id);
            if (_tasklist.CheckOwner(id) | _tasklist.CheckTeam(id))
            {
                return PartialView(taskList);
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                _tasklist.Edit(taskList.Id, taskList);
                return Json(new { success = true });
            }
            return PartialView(taskList);
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            if (_tasklist.CheckOwner(id) | _tasklist.CheckTeam(id))
            {
                await _tasklist.Delete(id);
                return RedirectToAction("Index", "TaskList");
            }
            return View("Error");
        }

        public async Task<ActionResult> AddMember(int id, string UserName)
        {
            await _project.AddTeamMember(id, UserName);
            return RedirectToAction("Index", "TaskList");
        }

        public async Task<ActionResult> DeleteMember(string id, int projectId)
        {
            await _project.DeleteTeamMember(id, projectId);
            return RedirectToAction("Index", "TaskList");
        }


    }
}
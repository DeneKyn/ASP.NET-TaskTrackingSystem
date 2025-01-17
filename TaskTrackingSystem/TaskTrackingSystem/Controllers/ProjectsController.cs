﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TaskTrackingSystem.ViewModels;
using TaskTrackingSystem.Models;
using Microsoft.EntityFrameworkCore;
using TaskTrackingSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace TaskTrackingSystem.Controllers
{
    public class ProjectsController : Controller
    {        
        private IProjectService _project;
        private IApplicationUser _user;
        private ApplicationContext db;

        public ProjectsController(IApplicationUser user, IProjectService project, ApplicationContext context)
        {
            _project = project;
            _user = user;
            db = context;
        }



        [Authorize]
        public ActionResult Index(string username)
        {          

            ApplicationUser user = _user.Get(username);

            ProjectViewModel model = new ProjectViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = user.UserName,
                Projects = _project.Get().ToList(),
                TeamProjects = _project.GetTeamProjects()
            };
            //return Json(model);
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Project project = _project.GetById(id);
            if (_user.Get().Id == project.UserId)
            {
                return PartialView(project);
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                _project.Edit(project.Id, project);
                return Json(new { success = true });
            }
            return PartialView(project);
        }

        [Authorize]
        public ActionResult Create()
        {            
            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Project project)
        {          
            if (ModelState.IsValid)
            {
                await _project.Create(project);
                return Json(new { success = true });
            }
            return PartialView(project);
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            if (_user.Get().Id == _project.GetById(id).UserId)
            { 
                await _project.Delete(id);
                return RedirectToAction("Index", "Projects");
            }
            return View("Error");
        }

        [Authorize] 
        public async Task<ActionResult> Chat(int id)
        {            
            var project = _project.GetById(id);
            if (_user.Get().Id == project.UserId)
            {
                var chat = db.Chats
                    .Include(x => x.Messages)
                        .ThenInclude(x => x.User)
                    .FirstOrDefault(x=> x.ProjectId == id);
                var chatmessage = db.ChatMessages;                
                return View(chat);
            }
            return View("Error");
        }

    }
}

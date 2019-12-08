using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext db;
        UserManager<ApplicationUser> _userManager;
        private int CurrentTaskListId;

        public TaskController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
            CurrentTaskListId = -1;

        }
        public async Task<IActionResult> IndexAsync(int id, int tl, int proj, string name)
        {
            ApplicationUser user = new ApplicationUser();
            if (name == null)
                name = User.Identity.Name;
            
            user = await _userManager.FindByNameAsync(name);

            var prpject = db.Projects                
                .Where(p => p.UserId == user.Id)
                .ToList()[proj];

            
            if (prpject.Status == ProjecrStatus.Private & name != User.Identity.Name)
            {
                return Content("Acced Denied");
            }

            var tasklist = db.TaskLists
                .Where(tl => tl.ProjectId == prpject.Id)
                .ToList()[tl];
            
            var task = db.ProjectTasks
                .Where(t => t.TaskListId == tasklist.Id)
                .ToList()[id];            
            

            return Json(task);
        }
    }
}

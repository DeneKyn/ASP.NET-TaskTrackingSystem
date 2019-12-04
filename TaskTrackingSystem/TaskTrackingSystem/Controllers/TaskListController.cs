using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class TaskListController : Controller
    {
        private ApplicationContext db;
        UserManager<ApplicationUser> _userManager;
        public TaskListController( ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }       
        public async Task<IActionResult> Index(int id, string name)
        {
            
            ApplicationUser user = new ApplicationUser();
            if (name == null)
                name = User.Identity.Name;

            user = await _userManager.FindByNameAsync(name);
            var Projects = db.Projects.Where(p => p.UserId == user.Id);
            var prpject = (Projects.ToList())[id];           


            var temp = (db.TaskLists.Where(p => p.ProjectId == prpject.Id)).ToList();
            foreach(TaskList taskList in temp)
            {
                var kek = db.ProjectTasks.Where(p => p.TaskListId == taskList.Id);
                taskList.ProjectTasks = kek.ToList();
            }
            return View(temp);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Controllers
{
    public class TaskListController : Controller
    {
        private ApplicationContext db;
        UserManager<ApplicationUser> _userManager;
        private int CurrentTaskListId; 
        
        public TaskListController(ApplicationContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
            CurrentTaskListId = -1;

        }


        public ActionResult Create(int id)
        {
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskList taskList, int id)
        {
            if (!ModelState.IsValid)
                return View(taskList);
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            taskList.Project = 
                db.Projects.Where(p => p.UserId == user.Id).ToList()        
                .FirstOrDefault(p => p.Id == id); 

            db.TaskLists.Add(new TaskList { Name = taskList.Name, Project = taskList.Project });
            db.SaveChanges();
            return RedirectToAction("Index", "TaskList");

        }

        public async Task<IActionResult> Index(int id, string name)
        {

            ApplicationUser user = new ApplicationUser();
            if (name == null)
                name = User.Identity.Name;

            user = await _userManager.FindByNameAsync(name);
            var Projects = db.Projects.Where(p => p.UserId == user.Id);
            var prpject = (Projects.ToList())[id];
            if (prpject.Status == ProjecrStatus.Private & name != User.Identity.Name)
            {
                return Content("Acced Denied");
            }

            CurrentTaskListId = prpject.Id;            
            ViewData["Message"] = prpject.Name;
            ViewBag.ProjectId = prpject.Id;
            var temp = (db.TaskLists.Where(p => p.ProjectId == prpject.Id)).ToList();
            foreach (TaskList taskList in temp)
            {
                var kek = db.ProjectTasks.Where(p => p.TaskListId == taskList.Id);
                taskList.ProjectTasks = kek.ToList();
            }
            return View(temp);
        }
    }
}
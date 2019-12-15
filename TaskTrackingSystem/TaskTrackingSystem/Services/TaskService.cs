using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Services
{
    
    public interface ITaskService
    {
        ProjectTask GetById(int id);
        Task Create(ProjectTask task, int TaskListId);
        Task ChangePosition(int id, int changeId);
        Project GetProject(int id);
        TaskList GetTaskList(int id);
    }
    public class TaskService : ITaskService
    {
        private readonly ApplicationContext _context;
        private IApplicationUser _user;
        private IProjectService _project;
        private ITaskListService _tasklist;

        public TaskService(ApplicationContext context, IApplicationUser user, IProjectService project, ITaskListService tasklist)
        {
            _context = context;
            _user = user;
            _project = project;
            _tasklist = tasklist;
        }

        public ProjectTask GetById(int id)
        {
            ProjectTask task = _context.ProjectTasks.FirstOrDefault(p => p.Id == id);
            return task;
        }       

        public async Task Create(ProjectTask task, int TaskListId)
        {
            ApplicationUser user = _user.Get();
            task.TaskList = _tasklist.GetById(TaskListId);
            _context.ProjectTasks.Add(new ProjectTask { Name = task.Name, Description = task.Description, Author = user, TaskList = task.TaskList });
            await _context.SaveChangesAsync();
        }

        public Project GetProject(int id)
        {
            ProjectTask task = GetById(id);
            TaskList tasklist = _tasklist.GetById(task.TaskListId);
            Project project = _project.GetAllById(tasklist.ProjectId);
            return project;
        }

        public TaskList GetTaskList(int id)
        {
            ProjectTask task = GetById(id);
            TaskList tasklist = _tasklist.GetById(task.TaskListId);            
            return tasklist;
        }

        public async Task ChangePosition(int id, int changeId)
        {
            var task = GetById(id);
            task.TaskListId = changeId;
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}

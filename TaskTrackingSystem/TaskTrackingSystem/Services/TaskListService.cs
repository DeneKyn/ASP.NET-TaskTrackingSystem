using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Services
{
    public interface ITaskListService
    {
        IEnumerable<TaskList> GetAllFull();
        IEnumerable<TaskList> GetFull(int id);
        TaskList GetById(int id);        
        Task Create(TaskList taskList, int ProjectId);
        Task Edit(int id, TaskList taskl);
        Task Delete(int id);
        bool Cheeck(int id);


    }
    public class TaskListService : ITaskListService
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationUser _user;
        private IProjectService _project;

        public TaskListService(ApplicationContext context, IApplicationUser user, IProjectService project)
        {
            _context = context;
            _user = user;
            _project = project;
        }
        public IEnumerable<TaskList> GetAll()
        {
            var tasklists = _context.TaskLists;
            return tasklists;
        }
        public IEnumerable<TaskList> GetAllFull()
        {
            var taskLists = _context.TaskLists            
                .Include(x => x.ProjectTasks)
                .ThenInclude(x => x.Author);
            return taskLists;
        }

        public IEnumerable<TaskList> GetFull(int id)
        {
            var taskLists = GetAllFull().Where(p => p.ProjectId == id);
            return taskLists;
        }
        public TaskList GetById(int id)
        {
            var taskList = GetAll()
                .FirstOrDefault(p => p.Id == id);
            return taskList;
        }

        public async Task Create(TaskList taskList, int ProjectId)
        {            
            _context.TaskLists.Add(new TaskList { 
                Name = taskList.Name, 
                Project = _project.GetById(ProjectId) });

            await _context.SaveChangesAsync();
        }

        public async Task Edit(int id, TaskList taskl)
        {
            var tasklist = GetById(id);
            tasklist.Name = taskl.Name;
            _context.TaskLists.Update(tasklist);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tasklist = GetById(id);
            _context.Remove(tasklist);
            await _context.SaveChangesAsync();
        }

        public bool Cheeck(int id)
        {
            ApplicationUser user = _user.Get();
            var taskList = GetById(id);
            var project = _project.GetById(taskList.ProjectId);
            if (user.Id == project.UserId)            
                return true;
            return false;
        }

    }
}

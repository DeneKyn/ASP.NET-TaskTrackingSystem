using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAllFull();
        IEnumerable<Project> GetFull(string username);
        IEnumerable<Project> GetAll();
        Project GetById(int id);
        IEnumerable<Project> Get(string username = null);
        Task Create(Project project);
        Task Delete(int id);
    }
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationUser _user;

        public ProjectService(ApplicationContext context, IApplicationUser user)
        {
            _context = context;
            _user = user;
        }

        public Project GetById(int id)
        {
            var project = GetAll()
                .FirstOrDefault(p => p.Id == id);
            return project;
        }
        public IEnumerable<Project> GetAllFull()
        {
            var projects = _context.Projects
                .Include(x => x.TaskLists)
                .ThenInclude(x => x.ProjectTasks)
                .ThenInclude(x => x.Author);
            return projects;
        }        

        public IEnumerable<Project> GetFull(string username = null)
        {            
            var user = _user.Get(username);
            var project = GetAllFull()
                .Where(p => p.UserId == user.Id);
            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = _context.Projects;       
            return projects;
        }
        public IEnumerable<Project> Get(string username = null)
        {
            var user = _user.Get(username);
            var project = GetAll()
                .Where(p => p.UserId == user.Id);
            return project;
        }

        public async Task Create(Project project)
        {
            ApplicationUser user = _user.Get();
            project.UserId = user.Id;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();            
        }

        public async Task Delete(int id)
        {
            var forum = GetById(id);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

    }
}

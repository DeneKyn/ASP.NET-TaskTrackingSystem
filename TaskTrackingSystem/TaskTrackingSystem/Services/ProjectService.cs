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
        Task Edit(int id, Project proj);
        List<UserProject> GetTeamProjects();
        bool CheckTeam(int id);
        bool CheckOwner(string userid);
        Task AddTeamMember(int id, string UserName);
        Task DeleteTeamMember(string id, int projectId);
        Project GetAllById(int id);
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

        public Project GetAllById(int id)
        {
            var project = GetAllFull()
                .FirstOrDefault(p => p.Id == id);
            return project;
        }
        public IEnumerable<Project> GetAllFull()
        {
            var projects = _context.Projects
                .Include(c => c.UserProjects)
                    .ThenInclude(sc => sc.User)
                .Include(c => c.UserProjects)
                    .ThenInclude(sc => sc.Project)
                    .ThenInclude(x => x.User)
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
            var projects = _context.Projects
                .Include(c => c.UserProjects)
                    .ThenInclude(sc => sc.User)
                .Include(c => c.UserProjects)
                    .ThenInclude(sc => sc.Project)
                    .ThenInclude(x => x.User);
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

        public async Task Edit(int id, Project proj)
        {
            var project = GetById(id);
            project.Name = proj.Name;
            project.Description = proj.Description;
            project.Status = proj.Status;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public List<UserProject> GetTeamProjects()
        {
            ApplicationUser user = _user.Get();
            var projects = GetAll();
            var kek = GetAll().Select(x => x.UserProjects);
            List<UserProject> TeamsProjects = new List<UserProject>();
            foreach (var lol in kek)            
                TeamsProjects.AddRange(lol.Where(x => x.UserId == user.Id));
           
            return TeamsProjects;
        }
        public bool CheckTeam(int id)
        {
            var result = GetTeamProjects()
                .Select(x => x.ProjectId)
                .ToList()
                .Contains(id);
            return result;
        }

        public bool CheckOwner(string userid)
        {

            var currentUser = _user.Get();
            var result = userid == currentUser.Id ? true : false;
                return result;
        }

        public async Task AddTeamMember(int id, string UserName)
        {
            ApplicationUser user = _user.Get(UserName);
            Project project = GetById(id);

            project.UserProjects.Add(new UserProject { ProjectId = project.Id, UserId = user.Id });            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTeamMember(string id, int projectId)
        {
            ApplicationUser user = _user.GetById(id);
            Project project = GetById(projectId);

            if (project != null && user != null)
            {
                var userProject = project.UserProjects.FirstOrDefault(sc => sc.UserId == user.Id);
                project.UserProjects.Remove(userProject);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}

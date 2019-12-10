using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.Services
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser Get(string name = null);
        /*IEnumerable<string> GetRoles(ApplicationUser user);*/
    }

    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUserService(ApplicationContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public ApplicationUser Get(string username = null)
        {
            ApplicationUser user;
            if (String.IsNullOrEmpty(username))            
                user = _userManager.Users.FirstOrDefault(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);            
            else            
                user = _userManager.Users.FirstOrDefault(u => u.UserName == username);            
            return user;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

        

        private int Json()
        {
            throw new NotImplementedException();
        }

        /*public IEnumerable<string> GetRoles(ApplicationUser user)
        {
            var roles = _roleManager.Roles.ToList();
            return
            var usuarios = usuarioManager.Users.Include(x => x.Roles).ToList();
            return _roleManager.
        }*/

    }
}

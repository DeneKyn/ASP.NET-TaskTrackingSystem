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
        ApplicationUser GetByName(string name);


        }


    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationContext _context;

        public ApplicationUserService(ApplicationContext context)
        {
            _context = context;
        }
        
        public ApplicationUser GetByName(string name)
        {
            return _context.Users.FirstOrDefault(user => user.UserName == name);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }
    }
}

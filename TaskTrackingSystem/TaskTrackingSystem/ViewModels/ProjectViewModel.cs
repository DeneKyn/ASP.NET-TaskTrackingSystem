using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.ViewModels
{
    public class ProjectViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public ProjecrStatus Status { get; set; }
        public IEnumerable<Project> Projects { get; set; }

    }
}

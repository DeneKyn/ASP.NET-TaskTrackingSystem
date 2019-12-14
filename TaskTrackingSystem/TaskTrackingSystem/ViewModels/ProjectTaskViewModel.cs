using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.ViewModels
{
    public class ProjectTaskViewModel
    {
        public ProjectTask CurrentTask { get; set; }        
        public int ChangeId { get; set; }
    }
}

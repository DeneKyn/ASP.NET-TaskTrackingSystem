using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        public string Description { get; set; }

        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; }
    }
}

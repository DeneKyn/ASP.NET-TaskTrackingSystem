using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class TaskList
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}

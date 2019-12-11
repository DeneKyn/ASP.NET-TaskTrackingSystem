using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class TaskList
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public IEnumerable<ProjectTask> ProjectTasks { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}

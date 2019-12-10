using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }        
        public string Name { get; set; }       
        public string Description { get; set; }
        public string  AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }


        public int TaskListId { get; set; }
        public TaskList TaskList { get; set; }
    }
}

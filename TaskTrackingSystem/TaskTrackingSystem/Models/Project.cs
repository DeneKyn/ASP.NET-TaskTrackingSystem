using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Models
{
    public enum ProjecrStatus
    {        
        Public,
        Private        
    }
    public class Project
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(10)")]        
        public ProjecrStatus Status { get; set; }

        public IEnumerable<TaskList> TaskLists { get; set; }

        public List<UserProject> UserProjects { get; set; }

        public Project()
        {
            UserProjects = new List<UserProject>();
        }
    }
}

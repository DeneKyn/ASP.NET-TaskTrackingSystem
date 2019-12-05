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
        Private,
        Team
    }
    public class Project
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(10)")]        
        public ProjecrStatus Status { get; set; }

        public List<TaskList> TaskLists { get; set; }
    }
}

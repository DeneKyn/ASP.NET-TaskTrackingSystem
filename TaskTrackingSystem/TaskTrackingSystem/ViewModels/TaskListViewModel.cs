using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTrackingSystem.Models;

namespace TaskTrackingSystem.ViewModels
{
    public class TaskListViewModel
    {
        public int ProjectId {get;set;}
        public string ProjectName { get; set; }
        public IEnumerable<TaskList> TaskLists { get; set; }
    }
}

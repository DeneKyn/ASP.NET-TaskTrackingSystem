﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskTrackingSystem.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
            Database.EnsureCreated();
        }

        public DbSet<Project> Projects { get; set; }       
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

    }
}

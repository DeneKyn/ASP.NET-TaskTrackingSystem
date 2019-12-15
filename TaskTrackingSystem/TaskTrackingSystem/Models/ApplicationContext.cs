using System;
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
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            

            builder.Entity<ProjectTask>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(f => f.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserProject>()
            .HasKey(t => new { t.UserId, t.ProjectId });

            builder.Entity<UserProject>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserProjects)
                .HasForeignKey(sc => sc.UserId);

            builder.Entity<UserProject>()
                .HasOne(sc => sc.Project)
                .WithMany(c => c.UserProjects)
                .HasForeignKey(sc => sc.ProjectId);

        }
    }
}

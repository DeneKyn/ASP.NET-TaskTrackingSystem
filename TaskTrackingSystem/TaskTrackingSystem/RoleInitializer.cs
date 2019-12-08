using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TaskTrackingSystem.Models;


namespace TaskTrackingSystem
{

    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            /* ApplicationUser user1 = new ApplicationUser { Email = "denekyn@gmail.com", UserName = "DeneKyn", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { Email = "AlexGuber@gmail.com", UserName = "Alex", EmailConfirmed = true };
            var result = await userManager.CreateAsync(user1, "Qwerty@228");
            await userManager.CreateAsync(user2, "Qwerty@228");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "admin");
            }

            Project project1 = new Project { Name = "Name 1 Project 1", Description = "Some info", UserId = user1.Id };
            Project project2 = new Project { Name = "Name 2 Project 1", Description = "Some info", UserId = user2.Id };
            Project project3 = new Project { Name = "My Project LolKek", Description = "Some info", UserId = user1.Id, Status = ProjecrStatus.Private };
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Projects.Add(project3);
            context.SaveChanges();

            TaskList lol = new TaskList { Name = "To Do", Project = project1 };
            TaskList kek = new TaskList { Name = "Done", Project = project1 };
            TaskList lolkek = new TaskList { Name = "In progress", Project = project2 };
            context.TaskLists.AddRange(lol, kek, lolkek);
            context.SaveChanges();

            ProjectTask task1 = new ProjectTask { Name = "Task1", Description = "LolJej", TaskListId = lol.Id };
            ProjectTask task2 = new ProjectTask { Name = "Task2", Description = "LolJej", TaskListId = lol.Id };
            ProjectTask task3 = new ProjectTask { Name = "Task3", Description = "LolJej", TaskListId = lol.Id };

            ProjectTask task4 = new ProjectTask { Name = "Task4", Description = "LolJej", TaskListId = kek.Id };
            ProjectTask task5 = new ProjectTask { Name = "Task5", Description = "LolJej", TaskListId = kek.Id };
            ProjectTask task6 = new ProjectTask { Name = "Task6", Description = "LolJej", TaskListId = kek.Id };

            ProjectTask task7 = new ProjectTask { Name = "Task7", Description = "LolJej", TaskListId = lolkek.Id };
            ProjectTask task8 = new ProjectTask { Name = "Task8", Description = "LolJej", TaskListId = lolkek.Id };
            ProjectTask task9 = new ProjectTask { Name = "Task9", Description = "LolJej", TaskListId = lolkek.Id };
            context.ProjectTasks.AddRange(task1, task2, task3, task5, task6, task7, task8, task9);
            context.SaveChanges();*/
        }
    }
    
}

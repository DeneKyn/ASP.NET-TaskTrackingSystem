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
            public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
            {
                
                if (await roleManager.FindByNameAsync("admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                }
                if (await roleManager.FindByNameAsync("user") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("user"));
                }                
            }
        }
    
}

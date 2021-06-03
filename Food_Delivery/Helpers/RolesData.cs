using Food_Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Helpers
{
    public static class RolesData
    {
        //ROLES HERE
        private static readonly string[] Roles = Role.GetAllRolesForSeed();

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var UserManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                foreach (var rloe in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(rloe.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole(rloe.ToString()));
                    }
                }
                // find the user with the admin email 
                var _user = await UserManager.FindByEmailAsync("Master@Master");
                // check if the user exists
                if (_user == null)
                {
                    //Here you could create the super admin who will maintain the web app
                    var poweruser = new ApplicationUser
                    {
                        UserName = "Master@Master",
                        Email = "Master@Master",
                        FirstName = "Admin",
                        LastName = ""
                    };
                    string adminPassword = "12345678";

                    var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
                    await UserManager.SetLockoutEnabledAsync(await UserManager.FindByEmailAsync("Master@Master"), false);//Master Never gets lockedout
                    if (createPowerUser.Succeeded)
                    {
                        ////Put here as a sample
                        //var x = UserManager.RemoveFromRoleAsync(poweruser, Role.Admin);

                        //Add the admin to his role
                        await UserManager.AddToRoleAsync(poweruser, Role.Admin);
                    }
                }
            }
        }
    }
}

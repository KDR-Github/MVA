using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityAuthorizationSample.Data
{
    public static class SampleData
    {
        public static async Task InitializeData(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("SampleData");

            using (var serviceScope = services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
                if (!env.IsDevelopment())
                    return;

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                //Create Roles
                var adminTask = roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                var powerUserTask = roleManager.CreateAsync(new IdentityRole { Name = "Power Users" });
                Task.WaitAll(adminTask, powerUserTask);
                logger.LogInformation("===> Added Admin and Power User Roles");

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                var user = new ApplicationUser
                {
                    Email = "kdr@test.com",
                    UserName = "kdr@test.com"
                };

                await userManager.CreateAsync(user, "Kdrkdr@123");

                logger.LogInformation($"==> Created user kdr@test.com with password Kdrkdr@123");

                //await userManager.AddToRoleAsync(user, "Admin");

                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Country, "Canada"));
            }
        }

    }
}

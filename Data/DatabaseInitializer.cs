
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using DotaBuffClone.Models;

namespace DotaBuffClone.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Apply migrations
                context.Database.EnsureCreated();

                // Seed roles
                if (!roleManager.Roles.Any())
                {
                    roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                    roleManager.CreateAsync(new IdentityRole("User")).Wait();
                }

                // Seed admin user
                if (!userManager.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@dotaclone.com"
                    };
                    userManager.CreateAsync(adminUser, "Admin123!").Wait();
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }

                // Seed Heroes
                if (!context.Heroes.Any())
                {
                    context.Heroes.AddRange(
                        new Hero { Name = "Anti-Mage", ImageUrl = "https://example.com/anti-mage.png" },
                        new Hero { Name = "Invoker", ImageUrl = "https://example.com/invoker.png" },
                        new Hero { Name = "Juggernaut", ImageUrl = "https://example.com/juggernaut.png" }
                    );
                }

                // Seed Items
                if (!context.Items.Any())
                {
                    context.Items.AddRange(
                        new Item { Name = "Blink Dagger", ImageUrl = "https://example.com/blink-dagger.png" },
                        new Item { Name = "Aghanim's Scepter", ImageUrl = "https://example.com/aghs.png" }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}

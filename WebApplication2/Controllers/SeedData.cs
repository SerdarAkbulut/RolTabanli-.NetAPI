using Microsoft.AspNetCore.Identity;
using WebApplication2.Entity;

namespace WebApplication2.Controllers
{
    public class SeedData
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            if (!roleManager.Roles.Any())
            {
                var employer = new Role { Name = "Employer" };
                var admin = new Role { Name = "Admin" };

                await roleManager.CreateAsync(employer);
                await roleManager.CreateAsync(admin);
            }

            if (!userManager.Users.Any())
            {
                var personel1 = new User { Name = "Personel1", UserName = "personel1", Email = "personel1@gmail.com" };
                var personel2 = new User { Name = "Personel2", UserName = "personel2", Email = "personel2@gmail.com" };
                var personel3 = new User { Name = "Personel3", UserName = "personel3", Email = "personel3@gmail.com" };

                await userManager.CreateAsync(personel1, "Personel_123");
                await userManager.AddToRoleAsync(personel1, "Employer");

                await userManager.CreateAsync(personel2, "Personel_123");
                await userManager.AddToRoleAsync(personel2, "Employer");

                await userManager.CreateAsync(personel3, "Personel_123");
                await userManager.AddToRoleAsync(personel3, "Employer");

                var admin = new User { Name = "Admin", UserName = "admin", Email = "admin@gmail.com" };
                await userManager.CreateAsync(admin, "Admin_123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}

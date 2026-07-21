using Microsoft.AspNetCore.Identity;
using TransLight.DataAccess.IdentityModel;

namespace TransLight.DataAccess.Seeder
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdmin(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            // Create Admin role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>("Admin"));
            }


            // Create Admin user
            var adminEmail = "admin@translight.com";

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };


                var result = await userManager.CreateAsync(
                    user,
                    "Admin@123");


                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        user,
                        "Admin");
                }
            }
        }
    }
}

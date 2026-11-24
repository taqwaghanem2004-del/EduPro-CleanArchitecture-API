using EduPro.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var usersCount = await userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var admin = new User()
                {
                    UserName = "admin",
                    Email = "admin@edupro.com",
                    FullName = "EduPro Admin",
                    PhoneNumber = "0000000000",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsBlocked = false
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}

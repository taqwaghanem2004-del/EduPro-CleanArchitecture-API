using EduPro.Data.Entities;
using EduPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Seeder
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
        new Category { Name = "Computer Science & Engineering", Description = "Shared courses between CS and CE" },
 new Category { Name = "Civil & Architectural Engineering", Description = "Shared courses between Civil, Building and Architecture" },
new Category { Name = "Electrical & Mechatronics & Energy Engineering", Description = "Shared courses between Electrical, Mechatronics and Energy" },
new Category { Name = "Automotive Engineering", Description = "Automotive Engineering courses" },
new Category { Name = "Mathematics", Description = "Calculus, Linear Algebra, Statistics, Discrete Math" },
new Category { Name = "Physics", Description = "Mechanics, Thermodynamics, Optics" },
new Category { Name = "Chemistry", Description = "General and Organic Chemistry" },
new Category { Name = "Medicine & Health Sciences", Description = "Medical and Health related courses" },
new Category { Name = "Humanities & Languages", Description = "Arabic, English, Technical Writing" },
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

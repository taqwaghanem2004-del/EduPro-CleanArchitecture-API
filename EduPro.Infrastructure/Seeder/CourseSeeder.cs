using EduPro.Data.Entities;
using EduPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Seeder
{
    public static class CourseSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!await dbContext.Courses.AnyAsync())
            {
                var categories = await dbContext.Categories.ToListAsync();

                var courses = new List<Course>
                {
                    // Computer Science & Engineering
                    new Course { Name = "Data Structures", Description = "Arrays, Linked Lists, Trees, and Graphs", CategoryId = categories[0].Id },
                    new Course { Name = "Algorithms", Description = "Sorting, Searching, and Algorithm Analysis", CategoryId = categories[0].Id },

                    // Civil & Architectural Engineering
                    new Course { Name = "Structural Analysis", Description = "Fundamentals of structural analysis and design", CategoryId = categories[1].Id },
                    new Course { Name = "Building Materials", Description = "Properties and uses of construction materials", CategoryId = categories[1].Id },

                    // Electrical & Mechatronics & Energy Engineering
                    new Course { Name = "Circuit Analysis", Description = "DC and AC circuit analysis fundamentals", CategoryId = categories[2].Id },
                    new Course { Name = "Electronics", Description = "Diodes, Transistors, and Amplifiers", CategoryId = categories[2].Id },

                    // Automotive Engineering
                    new Course { Name = "Engine Design", Description = "Internal combustion engine principles", CategoryId = categories[3].Id },
                    new Course { Name = "Vehicle Dynamics", Description = "Motion and control of vehicles", CategoryId = categories[3].Id },

                    // Mathematics
                    new Course { Name = "Calculus", Description = "Differential and Integral Calculus", CategoryId = categories[4].Id },
                    new Course { Name = "Linear Algebra", Description = "Matrices, Vectors, and Linear Transformations", CategoryId = categories[4].Id },

                    // Physics
                    new Course { Name = "Mechanics", Description = "Newton's laws and classical mechanics", CategoryId = categories[5].Id },
                    new Course { Name = "Thermodynamics", Description = "Heat, energy, and thermodynamic laws", CategoryId = categories[5].Id },

                    // Chemistry
                    new Course { Name = "General Chemistry", Description = "Atoms, molecules, and chemical reactions", CategoryId = categories[6].Id },
                    new Course { Name = "Organic Chemistry", Description = "Carbon compounds and organic reactions", CategoryId = categories[6].Id },

                    // Medicine & Health Sciences
                    new Course { Name = "Anatomy", Description = "Human body structure and systems", CategoryId = categories[7].Id },
                    new Course { Name = "Physiology", Description = "Functions of human body systems", CategoryId = categories[7].Id },

                    // Humanities & Languages
                    new Course { Name = "Technical Writing", Description = "Academic and technical writing skills", CategoryId = categories[8].Id },
                    new Course { Name = "English Language", Description = "English communication and grammar", CategoryId = categories[8].Id },
                };

                await dbContext.Courses.AddRangeAsync(courses);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepositoryAsync<Course>, ICourseRepository
    {
        #region Fields
        private readonly DbSet<Course> _courses;
        #endregion

        #region Constructors
        public CourseRepository(AppDbContext dbContext) : base(dbContext)
        {
            _courses = dbContext.Set<Course>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Course>> GetCoursesByCategoryIdAsync(int categoryId)
        {
            return await _courses
                .Include(x => x.Category)
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }
        #endregion
    }
}
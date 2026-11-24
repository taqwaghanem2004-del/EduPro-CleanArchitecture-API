using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Service.Implementations
{
    public class CourseService : ICourseService
    {
        #region Fields
        private readonly ICourseRepository _courseRepository;
        #endregion

        #region Constructors
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Course>> GetCoursesByCategoryIdAsync(int categoryId)
        {
            return await _courseRepository.GetCoursesByCategoryIdAsync(categoryId);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetTableNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsCourseNameExist(string name)
        {
            return await _courseRepository.GetTableNoTracking()
                .AnyAsync(x => x.Name == name);
        }

        public async Task<bool> IsCourseNameExistExcludeSelf(string name, int id)
        {
            return await _courseRepository.GetTableNoTracking()
                .AnyAsync(x => x.Name == name && x.Id != id);
        }

        public async Task<string> AddCourseAsync(Course course)
        {
            await _courseRepository.AddAsync(course);
            return "Success";
        }

        public async Task<string> EditCourseAsync(Course course)
        {
            await _courseRepository.UpdateAsync(course);
            return "Success";
        }

        public async Task<string> DeleteCourseAsync(Course course)
        {
            await _courseRepository.DeleteAsync(course);
            return "Success";
        }
        #endregion
    }
}
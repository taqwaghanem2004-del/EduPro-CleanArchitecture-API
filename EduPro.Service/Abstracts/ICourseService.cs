using EduPro.Data.Entities;

namespace EduPro.Service.Abstracts
{
    public interface ICourseService
    {
        public Task<List<Course>> GetCoursesByCategoryIdAsync(int categoryId);
        public Task<Course> GetCourseByIdAsync(int id);
        public Task<bool> IsCourseNameExist(string name);
        public Task<bool> IsCourseNameExistExcludeSelf(string name, int id);
        public Task<string> AddCourseAsync(Course course);
        public Task<string> EditCourseAsync(Course course);
        public Task<string> DeleteCourseAsync(Course course);
    }
}
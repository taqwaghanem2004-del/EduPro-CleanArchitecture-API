using EduPro.Data.Entities;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface ICourseRepository : IGenericRepositoryAsync<Course>
    {
        public Task<List<Course>> GetCoursesByCategoryIdAsync(int categoryId);
    }
}
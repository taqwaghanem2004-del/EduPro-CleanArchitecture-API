using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Repositories
{
    public class UserActivityRepository : GenericRepositoryAsync<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
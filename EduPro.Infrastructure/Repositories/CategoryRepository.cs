using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        #region Fields
        private DbSet<Category> _categories;
        #endregion

        #region Constructors
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _categories = dbContext.Set<Category>();
        }
        #endregion
    }
}

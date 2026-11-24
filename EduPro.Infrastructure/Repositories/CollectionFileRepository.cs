using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Repositories
{
    public class CollectionFileRepository : GenericRepositoryAsync<CollectionFile>, ICollectionFileRepository
    {
        public CollectionFileRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}

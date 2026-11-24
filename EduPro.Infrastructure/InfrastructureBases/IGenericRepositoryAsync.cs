using Microsoft.EntityFrameworkCore.Storage;

namespace EduPro.Infrastructure.InfrastructureBases
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task SaveChangesAsync();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();
    }
}

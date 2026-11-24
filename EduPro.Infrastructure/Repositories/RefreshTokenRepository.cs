using EduPro.Data.Entities.Identity;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        private readonly DbSet<UserRefreshToken> _userRefreshTokens;

        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
            _userRefreshTokens = dbContext.Set<UserRefreshToken>();
        }
    }
}

using EduPro.Data.Entities.Identity;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
    }
}

using EduPro.Data.Enums;

namespace EduPro.Service.Abstracts
{
    public interface IUserActivityService
    {
        Task LogAsync(int? userId, ActivityType activityType, string? details = null);
    }
}
using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;

namespace EduPro.Service.Implementations
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public UserActivityService(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public async Task LogAsync(int ? userId, ActivityType activityType, string? details = null)
        {
            if (!userId.HasValue) return;
            var activity = new UserActivity
            {
                UserId = userId.Value,
                ActivityType = activityType,
                Details = details,
                CreatedAt = DateTime.UtcNow
            };
            await _userActivityRepository.AddAsync(activity);
        }
    }
}
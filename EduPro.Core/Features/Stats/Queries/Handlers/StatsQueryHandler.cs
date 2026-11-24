using EduPro.Core.Bases;
using EduPro.Core.Features.Stats.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;
using EduPro.Data.Results;
using EduPro.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Stats.Queries.Handlers
{
    public class StatsQueryHandler : ResponseHandler,
         IRequestHandler<GetStatsQuery, Response<GetStatsResult>>,
         IRequestHandler<GetAdminStatsQuery, Response<GetAdminStatsResult>>
    {
        #region Fields
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public StatsQueryHandler(AppDbContext dbContext,
                                  UserManager<User> userManager,
                                  IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<GetStatsResult>> Handle(GetStatsQuery request, CancellationToken cancellationToken)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var result = new GetStatsResult
            {
                StudentsCount = students.Count,
                FilesCount = await _dbContext.EduFiles.CountAsync(),
                CoursesCount = await _dbContext.Courses.CountAsync(),
                CategoriesCount = await _dbContext.Categories.CountAsync(),
                TotalDownloads = await _dbContext.EduFiles.SumAsync(x => x.DownloadCount),
            };
            return Success(result);
        }

        public async Task<Response<GetAdminStatsResult>> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
        {
            var mostDownloadedFile = await _dbContext.EduFiles
                .Where(x => x.Status == EduFileStatus.Approved)
                .OrderByDescending(x => x.DownloadCount)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();

            var mostActiveCourse = await _dbContext.EduFiles
                .Where(x => x.Status == EduFileStatus.Approved)
                .GroupBy(x => x.Course.Name)
                .OrderByDescending(x => x.Sum(f => f.DownloadCount))
                .Select(x => x.Key)
                .FirstOrDefaultAsync();

            var result = new GetAdminStatsResult
            {
                PendingFilesCount = await _dbContext.EduFiles.CountAsync(x => x.Status == EduFileStatus.Pending),
                ApprovedFilesCount = await _dbContext.EduFiles.CountAsync(x => x.Status == EduFileStatus.Approved),
                RejectedFilesCount = await _dbContext.EduFiles.CountAsync(x => x.Status == EduFileStatus.Rejected),
                TotalDownloads = await _dbContext.EduFiles.SumAsync(x => x.DownloadCount),
                MostDownloadedFile = mostDownloadedFile,
                MostActiveCourse = mostActiveCourse
            };
            return Success(result);
        }
        #endregion
    }
}

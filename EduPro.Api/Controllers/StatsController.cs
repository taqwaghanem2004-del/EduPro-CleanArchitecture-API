using EduPro.Api.Base;
using EduPro.Core.Features.Stats.Queries.Models;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    public class StatsController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.StatsRouting.GetStats)]
        public async Task<IActionResult> GetStats()
        {
            return NewResult(await Mediator.Send(new GetStatsQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.StatsRouting.GetAdminStats)]
        public async Task<IActionResult> GetAdminStats()
        {
            return NewResult(await Mediator.Send(new GetAdminStatsQuery()));
        }
    }
}

using EduPro.Api.Base;
using EduPro.Core.Features.Collection.Commands.Models;
using EduPro.Core.Features.Favorite.Commands.Models;
using EduPro.Core.Features.Favorite.Queries.Models;
using EduPro.Core.Filters;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
    public class FavoriteController : AppControllerBase
    {
        [HttpGet(Router.FavoriteRouting.GetList)]
        public async Task<IActionResult> GetList()
            => NewResult(await Mediator.Send(new GetFavoriteListQuery()));

        [HttpPost(Router.FavoriteRouting.Add)]
        public async Task<IActionResult> Add([FromRoute] int eduFileId)
            => NewResult(await Mediator.Send(new AddFavoriteCommand(eduFileId)));

        [HttpDelete(Router.FavoriteRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int eduFileId)
            => NewResult(await Mediator.Send(new DeleteFavoriteCommand(eduFileId)));

        [HttpPost(Router.FavoriteRouting.AddCollectionToFavorites)]
        public async Task<IActionResult> AddCollectionToFavorites([FromRoute] int collectionId)
            => NewResult(await Mediator.Send(new AddCollectionToFavoritesCommand(collectionId)));

        [HttpDelete(Router.FavoriteRouting.RemoveCollectionFromFavorites)]
        public async Task<IActionResult> RemoveCollectionFromFavorites([FromRoute] int collectionId)
            => NewResult(await Mediator.Send(new RemoveCollectionFromFavoritesCommand(collectionId)));

        [HttpGet(Router.FavoriteRouting.GetFavoriteCollections)]
        public async Task<IActionResult> GetFavoriteCollections()
            => NewResult(await Mediator.Send(new GetFavoriteCollectionsQuery()));
    }
}
using EduPro.Api.Base;
using EduPro.Core.Features.Collection.Commands.Models;
using EduPro.Core.Features.Collection.Queries.Models;
using EduPro.Core.Filters;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Authorize]
[TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
public class CollectionController : AppControllerBase
{

    [AllowAnonymous]

    [HttpGet(Router.CollectionRouting.GetById)]
    public async Task<IActionResult> GetById([FromRoute] int id)
        => NewResult(await Mediator.Send(new GetCollectionByIdQuery(id)));


    [HttpPut(Router.CollectionRouting.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateCollectionCommand command)
        => NewResult(await Mediator.Send(command));

    [HttpDelete(Router.CollectionRouting.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int id)
        => NewResult(await Mediator.Send(new DeleteCollectionCommand(id)));

    [HttpPost(Router.CollectionRouting.AddFile)]
    public async Task<IActionResult> AddFile([FromBody] AddFileToCollectionCommand command)
        => NewResult(await Mediator.Send(command));

    [HttpDelete(Router.CollectionRouting.RemoveFile)]
    public async Task<IActionResult> RemoveFile([FromBody] RemoveFileFromCollectionCommand command)
        => NewResult(await Mediator.Send(command));

    [HttpPut(Router.CollectionRouting.Reorder)]
    public async Task<IActionResult> Reorder([FromBody] ReorderCollectionFilesCommand command)
        => NewResult(await Mediator.Send(command));

    [HttpPut(Router.CollectionRouting.Publish)]
    public async Task<IActionResult> Publish([FromRoute] int collectionId)
        => NewResult(await Mediator.Send(new PublishCollectionCommand(collectionId)));

    [AllowAnonymous]
    [HttpGet(Router.CollectionRouting.GetPublicCollections)]
    public async Task<IActionResult> GetPublicCollections()
        => NewResult(await Mediator.Send(new GetPublicCollectionsQuery()));
    [HttpPost(Router.CollectionRouting.Copy)]
    public async Task<IActionResult> Copy([FromRoute] int collectionId)
    => NewResult(await Mediator.Send(new CopyCollectionCommand(collectionId)));
    [HttpGet(Router.CollectionRouting.GetProfileCollections)]
    public async Task<IActionResult> GetProfileCollections()
       => NewResult(await Mediator.Send(new GetProfileCollectionsQuery()));
    [HttpGet(Router.CollectionRouting.GetLibraryCollections)]
    public async Task<IActionResult> GetLibraryCollections()
       => NewResult(await Mediator.Send(new GetLibraryCollectionsQuery()));
    [HttpPost(Router.CollectionRouting.CreateProfile)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCollectionCommand command)
      => NewResult(await Mediator.Send(command));
    [HttpPost(Router.CollectionRouting.CreateLibrary)]
    public async Task<IActionResult> CreateLibrary([FromBody] CreateLibraryCollectionCommand command)
       => NewResult(await Mediator.Send(command));
    [AllowAnonymous]
    [HttpGet(Router.CollectionRouting.GetByCourseId)]
    public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
    => NewResult(await Mediator.Send(new GetCollectionByCourseIdQuery(courseId)));
    [HttpPut(Router.CollectionRouting.Unpublish)]
    public async Task<IActionResult> Unpublish([FromRoute] int collectionId)
    => NewResult(await Mediator.Send(new UnpublishCollectionCommand(collectionId)));



}
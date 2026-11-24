using EduPro.Core.Bases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduPro.Api.Base
{
    [ApiController]
    public class AppControllerBase : ControllerBase
    {
        private IMediator _mediatorInstance;
        protected IMediator Mediator =>
            _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        public ObjectResult NewResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }
    }
}


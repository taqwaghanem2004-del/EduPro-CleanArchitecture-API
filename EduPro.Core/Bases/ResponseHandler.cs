using EduPro.Core.Resources;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Bases
{
    public class ResponseHandler
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ResponseHandler(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public Response<T> Success<T>(T entity, object meta = null)
        {
            return new Response<T>
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = _localizer[SharedResourcesKeys.Success],
                Meta = meta
            };
        }

        public Response<T> Created<T>(T entity, object meta = null)
        {
            return new Response<T>
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = _localizer[SharedResourcesKeys.Created],
                Meta = meta
            };
        }

        public Response<T> Deleted<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? _localizer[SharedResourcesKeys.Deleted]
            };
        }

        public Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? _localizer[SharedResourcesKeys.NotFound]
            };
        }

        public Response<T> BadRequest<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message ?? _localizer[SharedResourcesKeys.BadRequest]
            };
        }

        public Response<T> Unauthorized<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message ?? _localizer[SharedResourcesKeys.UnAuthorized]
            };
        }

        public Response<T> UnprocessableEntity<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message ?? _localizer[SharedResourcesKeys.UnprocessableEntity]
            };
        }
    }
}

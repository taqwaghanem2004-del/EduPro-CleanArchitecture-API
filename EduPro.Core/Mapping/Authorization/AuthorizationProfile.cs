using AutoMapper;
using EduPro.Core.Features.Authorization.Quaries.Results;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Results.Authorization;

namespace EduPro.Core.Mapping.Authorization
{
    public class AuthorizationProfile : Profile
    {
        public AuthorizationProfile()
        {
            CreateMap<Role, GetRolesListResult>();
            CreateMap<Role, GetRoleByIdResult>();
        }
    }
}
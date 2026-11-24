using AutoMapper;
using EduPro.Core.Features.Authentication.Commands.Models;
using EduPro.Data.Entities.Identity;

namespace EduPro.Core.Mapping.Authentication
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<RegisterCommand, User>()
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}


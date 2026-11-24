using AutoMapper;

namespace EduPro.Core.Mapping.UserProfile
{
    public partial class ProfileProfile : Profile
    {
        public ProfileProfile()
        {
            GetMyProfileMapping();
        }
    }
}
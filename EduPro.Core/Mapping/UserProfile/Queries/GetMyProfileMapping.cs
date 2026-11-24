using EduPro.Data.Results.Profile;

namespace EduPro.Core.Mapping.UserProfile
{
    public partial class ProfileProfile
    {
        public void GetMyProfileMapping()
        {
            CreateMap<Data.Entities.EduFile, MyFileResult>()
                .ForMember(d => d.CourseName,
                    o => o.MapFrom(s => s.Course.Name))
                .ForMember(d => d.CategoryName,
                    o => o.MapFrom(s => s.Course.Category.Name))
                .ForMember(d => d.IsPublished,
                    o => o.MapFrom(s => s.IsPublished));

            CreateMap<Data.Entities.Collection, MyCollectionResult>()
                .ForMember(d => d.FilesCount,
                    o => o.MapFrom(s => s.CollectionFiles.Count))
                .ForMember(d => d.CourseName,
                    o => o.MapFrom(s => s.Course != null ? s.Course.Name : null));
        }
    }
}
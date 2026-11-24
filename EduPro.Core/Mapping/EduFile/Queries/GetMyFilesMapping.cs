using EduPro.Data.Results.EduFile;

namespace EduPro.Core.Mapping.EduFile
{
    public partial class EduFileProfile
    {
        public void GetMyFilesMapping()
        {
            CreateMap<Data.Entities.EduFile, GetMyFilesResult>()
                .ForMember(d => d.CourseName,
                    o => o.MapFrom(s => s.Course.Name))
                .ForMember(d => d.CategoryName,
                    o => o.MapFrom(s => s.Course.Category.Name));
        }
    }
}

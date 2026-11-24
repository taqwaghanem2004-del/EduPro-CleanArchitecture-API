using EduPro.Core.Features.EduFile.Commands.Models;


namespace EduPro.Core.Mapping.EduFile
{
    public partial class EduFileProfile
    {
        public void UploadEduFileMapping()
        {
            CreateMap<UploadEduFileCommand, Data.Entities.EduFile>()
                .ForMember(dest => dest.FilePath, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedByUserId, opt => opt.Ignore());
        }
    }
}
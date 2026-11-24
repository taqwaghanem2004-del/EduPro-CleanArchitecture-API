using AutoMapper;

namespace EduPro.Core.Mapping.EduFile
{
    public partial class EduFileProfile : Profile
    {
        public EduFileProfile()
        {
            GetEduFileListMapping();
            GetEduFileByIdMapping();
            UploadEduFileMapping();
            GetMyFilesMapping();

        }
    }
}

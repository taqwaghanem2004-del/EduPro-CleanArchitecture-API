using AutoMapper;

namespace EduPro.Core.Mapping.Category
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            GetCategoryListMapping();
            GetCategoryByIdMapping();
            CreateCategoryMapping();
            UpdateCategoryMapping();
        }
    }
}

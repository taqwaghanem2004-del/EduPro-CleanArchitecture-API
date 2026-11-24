using EduPro.Core.Features.Category.Queries.Results;

namespace EduPro.Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void GetCategoryByIdMapping()
        {
            CreateMap<EduPro.Data.Entities.Category, GetCategoryListResponse>();
        }
    }
}
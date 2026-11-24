using EduPro.Core.Features.Category.Commands.Models;

namespace EduPro.Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void CreateCategoryMapping()
        {
            CreateMap<CreateCategoryCommand, EduPro.Data.Entities.Category>();
        }
    }
}
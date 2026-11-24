using EduPro.Core.Features.Category.Commands.Models;

namespace EduPro.Core.Mapping.Category
{
    public partial class CategoryProfile
    {
        public void UpdateCategoryMapping()
        {
            CreateMap<UpdateCategoryCommand, EduPro.Data.Entities.Category>();
        }
    }
}
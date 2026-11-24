using EduPro.Data.Entities;

namespace EduPro.Service.Abstracts
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoryListAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<string> CreateCategoryAsync(Category category);
        Task<string> UpdateCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(Category category);
        Task<bool> IsNameExist(string name);
        Task<bool> IsNameExistExcludeSelf(string name, int id);
        Task<bool> IsCategoryIdExist(int id);
    }
}

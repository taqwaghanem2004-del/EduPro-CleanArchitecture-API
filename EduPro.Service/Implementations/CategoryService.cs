using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Constructors
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Category>> GetCategoryListAsync()
        {
            return await _categoryRepository.GetTableNoTracking().ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetTableNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return "Success";
        }

        public async Task<string> UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            return "Success";
        }

        public async Task<string> DeleteCategoryAsync(Category category)
        {
            await _categoryRepository.DeleteAsync(category);
            return "Success";
        }

        public async Task<bool> IsNameExist(string name)
        {
            return await _categoryRepository.GetTableNoTracking()
                                            .AnyAsync(x => x.Name == name);
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            return await _categoryRepository.GetTableNoTracking()
                                            .AnyAsync(x => x.Name == name && x.Id != id);
        }
        public async Task<bool> IsCategoryIdExist(int id)
        {
            return await _categoryRepository.GetTableNoTracking()
                                            .AnyAsync(x => x.Id == id);
        }
        #endregion
    }
}

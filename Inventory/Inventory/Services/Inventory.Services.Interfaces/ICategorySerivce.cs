using ReverseAnalytics.Domain.DTOs.ProductCategory;

namespace Inventory.Services.Interfaces
{
    public interface ICategorySerivce
    {
        public Task<IEnumerable<ProductCategoryDto>> GetCategoriesAsync();
        public Task<ProductCategoryDto?> GetCategoryByIdAsync(int id);
        public Task<ProductCategoryDto?> CreateCategoryAsync(ProductCategoryForCreateDto categoryToCreate);
        public Task UpdateCategoryAsync(ProductCategoryForUpdateDto categoryToUpdate);
        public Task DeleteCategoryAsync(int id);
    }
}

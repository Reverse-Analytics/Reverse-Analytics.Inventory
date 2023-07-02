using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface ICategorySerivce
    {
        public Task<IEnumerable<ProductCategory>> GetCategoriesAsync();
        public Task<ProductCategory?> GetCategoryByIdAsync(int id);
        public Task<ProductCategory?> CreateCategoryAsync(ProductCategory categoryToCreate);
        public Task UpdateCategoryAsync(ProductCategory categoryToUpdate);
        public Task DeleteCategoryAsync(int id);
    }
}

using Inventory.Core.Models;

namespace Inventory.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>?> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Product?> CreateProductAsync(Product productToCreate);
        Task UpdateProductAsync(Product productToUpdate);
        Task DeleteProductAsync(int productId);
    }
}

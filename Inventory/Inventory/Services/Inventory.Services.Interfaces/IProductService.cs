using ReverseAnalytics.Domain.DTOs.Product;

namespace Inventory.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>?> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<ProductDto?> CreateProductAsync(ProductForCreateDto productToCreate);
        Task UpdateProductAsync(ProductForUpdateDto productToUpdate);
        Task DeleteProductAsync(int productId);
    }
}

using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;
using ReverseAnalytics.Domain.DTOs.Product;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        private readonly RestClientBase _client;

        public ProductService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ProductDto>?> GetProductsAsync()
        {
            var response = await _client.Get("products?pageSize=0&pageNumber=0");
            IEnumerable<ProductDto>? products = null;

            if (response is not null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

                products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(json, settings)?.ToList();
            }

            return products ?? Enumerable.Empty<ProductDto>();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var response = await _client.Get($"products/{productId}");
            
            if (response is not null && response.IsSuccessStatusCode)
            {
                var jsonReponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductDto?>(jsonReponse);
            }
            else
            {
                throw new Exception(response?.Content.ToString());
            }
        }

        public async Task<ProductDto?> CreateProductAsync(ProductForCreateDto productToCreate)
        {
            var json = JsonConvert.SerializeObject(productToCreate);
            var response = await _client.Post("products", json);

            if(response is not null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductDto>(jsonResponse);
            }
            else
            {
                throw new Exception(response?.Content.ToString());
            }
        }

        public async Task UpdateProductAsync(ProductForUpdateDto productToUpdate)
        {
            var json = JsonConvert.SerializeObject(productToUpdate);
            var response = await _client.Put("products", json);

            if(response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception(response?.Content.ToString());
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _client.Delete($"products/{productId}");
        }
    }
}

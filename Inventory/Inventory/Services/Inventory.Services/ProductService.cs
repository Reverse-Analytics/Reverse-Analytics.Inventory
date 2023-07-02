using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        private readonly RestClientBase _client;

        public ProductService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Product>?> GetProductsAsync()
        {
            var response = await _client.Get("products?pageSize=0&pageNumber=0");
            IEnumerable<Product>? products = null;

            if (response is not null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json, settings)?.ToList();
            }

            return products ?? Enumerable.Empty<Product>();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var response = await _client.Get($"products/{productId}");

            if (response is not null && response.IsSuccessStatusCode)
            {
                var jsonReponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product?>(jsonReponse);
            }
            else
            {
                throw new Exception(response?.Content.ToString());
            }
        }

        public async Task<Product?> CreateProductAsync(Product productToCreate)
        {
            var json = JsonConvert.SerializeObject(productToCreate);
            var response = await _client.Post("products", json);

            if (response is not null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(jsonResponse);
            }
            else
            {
                throw new Exception(response?.Content.ToString());
            }
        }

        public async Task UpdateProductAsync(Product productToUpdate)
        {
            var json = JsonConvert.SerializeObject(productToUpdate);
            var response = await _client.Put($"products/{productToUpdate.Id}", json);

            if (response is null || !response.IsSuccessStatusCode)
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

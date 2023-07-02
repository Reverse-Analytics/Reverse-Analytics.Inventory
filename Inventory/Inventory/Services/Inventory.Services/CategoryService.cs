using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class CategoryService : ICategorySerivce
    {
        private readonly RestClientBase _restClient;

        public CategoryService(RestClientBase restClient)
        {
            _restClient = restClient;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategoriesAsync()
        {
            var response = await _restClient.Get("categories");
            IEnumerable<ProductCategory>? result = null;

            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<IEnumerable<ProductCategory>>(json)?.ToList();
            }

            return result ?? Enumerable.Empty<ProductCategory>();
        }

        public async Task<ProductCategory?> GetCategoryByIdAsync(int id)
        {
            var response = await _restClient.Get($"categories/{id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductCategory>(json);
            }

            return null;
        }

        public async Task<ProductCategory?> CreateCategoryAsync(ProductCategory categoryToCreate)
        {
            var json = JsonConvert.SerializeObject(categoryToCreate);

            var response = await _restClient.Post("categories", json);
            if (response is not null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductCategory>(jsonResponse);
            }

            return null;
        }

        public async Task UpdateCategoryAsync(ProductCategory categoryToUpdate)
        {
            var json = JsonConvert.SerializeObject(categoryToUpdate);

            var response = await _restClient.Put($"categories/{categoryToUpdate.Id}", json);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _restClient.Delete($"categories/{id}");
        }
    }
}

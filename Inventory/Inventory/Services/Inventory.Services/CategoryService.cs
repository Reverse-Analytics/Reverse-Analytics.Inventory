using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;
using ReverseAnalytics.Domain.DTOs.ProductCategory;

namespace Inventory.Services
{
    public class CategoryService : ICategorySerivce
    {
        private readonly RestClientBase _restClient;

        public CategoryService(RestClientBase restClient)
        {
            _restClient = restClient;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetCategoriesAsync()
        {
            var response = await _restClient.Get("categories");
            IEnumerable<ProductCategoryDto>? result = null;

            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<IEnumerable<ProductCategoryDto>>(json)?.ToList();
            }

            return result ?? Enumerable.Empty<ProductCategoryDto>();
        }

        public async Task<ProductCategoryDto?> GetCategoryByIdAsync(int id)
        {
            var response = await _restClient.Get($"categories/{id}");
            if (response != null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductCategoryDto>(json);
            }

            return null;
        }

        public async Task<ProductCategoryDto?> CreateCategoryAsync(ProductCategoryForCreateDto categoryToCreate)
        {
            var json = JsonConvert.SerializeObject(categoryToCreate);

            var response = await _restClient.Post("categories", json);
            if (response is not null && response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductCategoryDto>(jsonResponse);
            }

            return null;
        }

        public async Task UpdateCategoryAsync(ProductCategoryForUpdateDto categoryToUpdate)
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

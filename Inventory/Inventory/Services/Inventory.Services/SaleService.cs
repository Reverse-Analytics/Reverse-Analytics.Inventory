using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class SaleService : ISaleService
    {
        private readonly RestClientBase _client;
        private const string url = "sales";

        public SaleService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<Sale> CreateSale(Sale saleToCreate)
        {
            var json = JsonConvert.SerializeObject(saleToCreate);

            var response = await _client.Post(url, json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error creating new sale.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Sale>(jsonResponse);

            if (result is null)
            {
                throw new JsonSerializationException(jsonResponse);
            }

            return result;
        }

        public async Task DeleteSale(int id)
        {
            var response = await _client.Delete($"{url}/id");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error deleting sale with id: {id}");
            }
        }

        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            var response = await _client.Get(url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error retrieving sales.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Sale>>(jsonResponse);

            return result ?? Enumerable.Empty<Sale>();
        }

        public async Task<Sale> GetById(int id)
        {
            var response = await _client.Get($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retrieving sale with id: {id}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Sale>(jsonResponse);

            if (result is null)
            {
                throw new JsonSerializationException("Error converting sale.");
            }

            return result;
        }

        public async Task UpdateSale(Sale saleToUpdate)
        {
            var json = JsonConvert.SerializeObject(saleToUpdate);

            var response = await _client.Put($"{url}/{saleToUpdate.Id}", json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error updating sale with id: {saleToUpdate.Id}");
            }
        }

        public async Task<IEnumerable<Sale>> GetByCustomerId(int customerId)
        {
            string path = $"{url}/customers/{customerId}";
            var response = await _client.Get(path);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retrieving sale with id: {customerId}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Sale>>(jsonResponse);

            if (result is null)
            {
                throw new JsonSerializationException("Error converting sale.");
            }

            return result;
        }
    }
}

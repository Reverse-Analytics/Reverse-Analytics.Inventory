using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class SaleDebtService : ISaleDebtService
    {
        private readonly RestClientBase _client;
        private const string url = "debts/sales";

        public SaleDebtService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<SaleDebt> CreateSaleDebtAsync(SaleDebt saleDebtToCreate)
        {
            var json = JsonConvert.SerializeObject(saleDebtToCreate);

            var response = await _client.Post(url, json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error creating new sale debt.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaleDebt>(jsonResponse);

            return result is null ? throw new JsonSerializationException(jsonResponse) : result;
        }

        public async Task DeleteSaleDebtAsync(int id)
        {
            var response = await _client.Delete($"{url}/id");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error deleting sale debt with id: {id}");
            }
        }

        public async Task<SaleDebt> GetSaleDebtByIdAsync(int id)
        {
            var response = await _client.Get($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retrieving sale debt with id: {id}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaleDebt>(jsonResponse);

            if (result is null)
            {
                throw new JsonSerializationException("Error converting sale debt.");
            }

            return result;
        }

        public async Task<IEnumerable<SaleDebt>> GetSaleDebtsAsync()
        {
            var response = await _client.Get(url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error retreiving sales debts with status code {response?.StatusCode}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<SaleDebt>>(jsonResponse);

            return result ?? Enumerable.Empty<SaleDebt>();
        }

        public async Task UpdateSaleDebtAsync(SaleDebt saleDebtToUpdate)
        {

            var json = JsonConvert.SerializeObject(saleDebtToUpdate);

            var response = await _client.Put($"{url}/{saleDebtToUpdate.Id}", json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error updating debt with id: {saleDebtToUpdate.Id}");
            }
        }

        public async Task<SaleDebt> CloseDebtAsync(int id)
        {
            var response = await _client.Put($"{url}/{id}/settle");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error closing debt with id: {id}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SaleDebt>(jsonResponse);

        }
        public async Task<SaleDebt> PayDebtAsync(int id, decimal amount)
        {
            var json = JsonConvert.SerializeObject(new
            {
                Id = id,
                Amount = amount
            });

            var response = await _client.Put($"{url}/{id}/payment", json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error making a payment to debt with id: {id} and amount: {amount}.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SaleDebt>(jsonResponse);
        }
    }
}

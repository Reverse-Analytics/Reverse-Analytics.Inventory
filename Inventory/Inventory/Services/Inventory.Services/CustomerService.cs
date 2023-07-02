using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly RestClientBase _client;
        private readonly static string url = "customers";

        public CustomerService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Customer>?> GetCustomersAsync()
        {
            var response = await _client.Get(url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseJson);

            return result;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var response = await _client.Get($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Customer>(responseJson);

            return result;
        }

        public async Task<Customer?> CreateCustomerAsync(Customer customerToCreate)
        {
            var json = JsonConvert.SerializeObject(customerToCreate);
            var response = await _client.Post(url: url, json: json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Customer>(responseJson);

            return result;
        }

        public async Task UpdateCustomerAsync(Customer customerToUpdate)
        {
            var json = JsonConvert.SerializeObject(customerToUpdate);
            var response = await _client.Put($"{url}/{customerToUpdate.Id}", json);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var response = await _client.Delete($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
    }
}

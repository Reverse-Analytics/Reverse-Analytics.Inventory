using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;

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

        public async Task<IEnumerable<CustomerDto>?> GetCustomersAsync()
        {
            var response = await _client.Get(url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CustomerDto>>(responseJson);

            return result;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var response = await _client.Get($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CustomerDto>(responseJson);

            return result;
        }

        public async Task<CustomerDto?> CreateCustomerAsync(CustomerForCreateDto customerToCreate)
        {
            var json = JsonConvert.SerializeObject(customerToCreate);
            var response = await _client.Post(json, url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CustomerDto>(responseJson);

            return result;
        }

        public async Task UpdateCustomerAsync(CustomerForUpdateDto customerToUpdate)
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

using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;
using ReverseAnalytics.Domain.DTOs.Debt;

namespace Inventory.Services
{
    public class DebtService : IDebtService
    {
        private readonly RestClientBase _client;
        private readonly string url = "debts";


        public DebtService(RestClientBase client)
        {
            _client = client;
        }

        public async Task<DebtDto?> GetDebtByIdAsync(int id)
        {
            var response = await _client.Get($"{url}/{id}");

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DebtDto>(responseJson);

            return result;
        }

        public async Task<IEnumerable<DebtDto>> GetDebtsAsync()
        {
            var response = await _client.Get(url);

            if (response is null || !response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<DebtDto>>(responseJson);

            return result ?? new List<DebtDto>();
        }
    }
}
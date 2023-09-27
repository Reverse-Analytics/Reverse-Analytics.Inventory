using Inventory.Core.Models;
using Inventory.RestClient;
using Inventory.Services.Interfaces;
using Newtonsoft.Json;

namespace Inventory.Services
{
    public class RefundService : IRefundService
    {
        private readonly RestClientBase _client;
        private readonly JsonSerializerSettings jsonSerializer;

        public RefundService(RestClientBase client)
        {
            _client = client;
            jsonSerializer = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        public async Task<IEnumerable<Refund>> GetRefundsAsync()
        {
            var response = await _client.Get("refunds");

            if (response is not null && response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<Refund>>(json, jsonSerializer);

                return result ?? Enumerable.Empty<Refund>();
            }

            throw new HttpRequestException("Error retreiving refunds data.", null, response?.StatusCode);
        }

        public async Task<Refund> GetRefundByIdAsync(int id)
        {
            var response = await _client.Get($"refunds/{id}");

            if (response?.IsSuccessStatusCode is not true)
            {
                throw new HttpRequestException("Error retreiving refunds data.", null, response?.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Refund>(json, jsonSerializer);

            return result ?? throw new JsonSerializationException("Error serializing Refund");
        }

        public async Task<Refund> CreateRefundAsync(Refund refundToCreate)
        {
            var json = JsonConvert.SerializeObject(refundToCreate);

            var response = await _client.Post("refunds", json);

            if (response?.IsSuccessStatusCode is not true)
            {
                throw new HttpRequestException($"Error creating Refund", null, response?.StatusCode);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Refund>(responseJson, jsonSerializer);

            return result ?? throw new JsonSerializationException("Error serializing created Refund.");
        }

        public async Task<Refund> UpdateRefundAsync(Refund refundToUpdate)
        {
            var json = JsonConvert.SerializeObject(refundToUpdate);
            var response = await _client.Put($"refunds/{refundToUpdate.Id}", json);

            if (response?.IsSuccessStatusCode is not true)
            {
                throw new HttpRequestException($"Error updating Refund, id: {refundToUpdate.Id}", null, response?.StatusCode);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Refund>(responseJson);

            return result ?? throw new JsonSerializationException($"Error serializing updated Refund");
        }

        public async Task DeleteRefundAsync(int id)
        {
            var response = await _client.Delete($"refunds/{id}");

            if (response?.IsSuccessStatusCode is not true)
            {
                throw new HttpRequestException($"Error deleting Refund with id: {id}", null, response?.StatusCode);
            }
        }
    }
}

using System.Net.Http.Headers;
using System.Text;

namespace Inventory.RestClient
{
    public class RestClientBase
    {
        private const string urlBase = "https://wpqmn9m6-7231.euw.devtunnels.ms/api";
        //private const string urlBase = "https://d3vxvbnt-7231.euw.devtunnels.ms/api";
        //private const string urlBase = "https://mj6vmt9t-7231.asse.devtunnels.ms/api";
        private readonly HttpClient client;

        public RestClientBase()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(urlBase);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage?> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress?.AbsoluteUri}/{url}");
            var response = await client.SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage?> Post(string url, string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseAddress?.AbsoluteUri}/{url}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage?> Put(string url, string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{client.BaseAddress?.AbsoluteUri}/{url}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage?> Delete(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{client.BaseAddress?.AbsoluteUri}/{url}");
            var response = await client.SendAsync(request);

            return response;
        }
    }
}

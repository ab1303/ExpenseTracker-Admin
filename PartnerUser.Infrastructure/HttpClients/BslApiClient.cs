using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PartnerUser.Common;

namespace PartnerUser.Infrastructure.HttpClients
{
    public class BslApiClient : IBslApiClient
    {
        private readonly HttpClient _httpClient;

        public BslApiClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(StackVariables.BslBaseAddress);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> SendAsync(
            string relativeUrl,
            HttpMethod httpMethod,
            HttpContent httpContent = null,
            string token = "")
        {
            var resourceUri = new Uri(_httpClient.BaseAddress, relativeUrl);
            var request = CreateRequestMessage(resourceUri, httpMethod, httpContent, token);
            return _httpClient.SendAsync(request);
        }

        private HttpRequestMessage CreateRequestMessage(
            Uri resourceUri,
            HttpMethod httpMethod,
            HttpContent httpContent = null,
            string token = "")
        {
            var request = new HttpRequestMessage
            {
                RequestUri = resourceUri,
                Method = httpMethod,
                Content = httpContent
            };
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return request;
        }
    }
}

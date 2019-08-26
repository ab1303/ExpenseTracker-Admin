using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Newtonsoft.Json;

namespace Admin.Api.IntegrationTests.RestClients
{
    public class UserClient : ClientBase
    {
        private readonly HttpClient _httpClient;
        private const string RequestBaseUri = "api/v1/Users";

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest request)
        {
            return await _httpClient.PostAsync(RequestBaseUri, GetJsonStringContent(request));
        }

        public async Task<HttpResponseMessage> GetUserAsync(Guid userId, string token)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{RequestBaseUri}/{userId}"),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token}" }
                }
            };

            return await _httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> GetUserListAsync(Guid UserGuid, string token)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{RequestBaseUri}?UserGuid={UserGuid}"),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token}" }
                }
            };

            return await _httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> UpdateUserAsync(Guid userId, 
            UpdateUserRequest request, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return await _httpClient.PutAsync($"{RequestBaseUri}/{userId}", GetJsonStringContent(request, jsonSerializerSettings));
        }
    }
}

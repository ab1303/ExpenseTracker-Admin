using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Newtonsoft.Json;

namespace Admin.Api.IntegrationTests.RestClients
{
    public class PartnerUserClient : ClientBase
    {
        private readonly HttpClient _httpClient;
        private const string RequestBaseUri = "api/v1/PartnerUsers";

        public PartnerUserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreatePartnerUserAsync(CreatePartnerUserRequest request)
        {
            return await _httpClient.PostAsync(RequestBaseUri, GetJsonStringContent(request));
        }

        public async Task<HttpResponseMessage> GetPartnerUserAsync(Guid partnerUserId, string token)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{RequestBaseUri}/{partnerUserId}"),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token}" }
                }
            };

            return await _httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> GetPartnerUserListAsync(Guid ofxUserGuid, Guid partnerAppId, string token)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_httpClient.BaseAddress}{RequestBaseUri}?ofxUserGuid={ofxUserGuid}&partnerAppId={partnerAppId}"),
                Headers = {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {token}" }
                }
            };

            return await _httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> UpdatePartnerUserAsync(Guid partnerUserId, 
            UpdatePartnerUserRequest request, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return await _httpClient.PutAsync($"{RequestBaseUri}/{partnerUserId}", GetJsonStringContent(request, jsonSerializerSettings));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Api.IntegrationTests.RestResponse;
using Newtonsoft.Json;

namespace Admin.Api.IntegrationTests.RestClients
{
    public class AuthServerClient
    {
        private readonly HttpClient _httpClient;

        public AuthServerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetApigeeClientCredentialsToken(Guid userGuid)
        {
            var httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "apigee"),
                new KeyValuePair<string, string>("client_secret", "12345"),
                new KeyValuePair<string, string>("grant_type", "apigee"),
                new KeyValuePair<string, string>("scope", "ALLAPI"),
                new KeyValuePair<string, string>("user_guid", userGuid.ToString())
            });

            var responseMessage = await _httpClient.PostAsync("/connect/token", httpContent);

            if (responseMessage.IsSuccessStatusCode == false)
            {
                throw new ApplicationException($"Failed to get access token from {_httpClient.BaseAddress} http code {responseMessage.StatusCode}");
            }

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var tokenResponse = JsonConvert.DeserializeObject<ClientCredentialsTokenResponse>(responseContent);

            return tokenResponse.AccessToken;
        }
    }
}

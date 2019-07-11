using Newtonsoft.Json;

namespace Admin.Api.IntegrationTests.RestResponse
{
    public class ClientCredentialsTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}

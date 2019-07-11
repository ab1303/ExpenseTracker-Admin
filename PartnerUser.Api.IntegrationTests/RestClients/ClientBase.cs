using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace PartnerUser.Api.IntegrationTests.RestClients
{
    public abstract class ClientBase
    {
        public StringContent GetJsonStringContent(object request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public StringContent GetJsonStringContent(object request, JsonSerializerSettings jsonSerializerSettings)
        {
            var json = JsonConvert.SerializeObject(request, jsonSerializerSettings);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}

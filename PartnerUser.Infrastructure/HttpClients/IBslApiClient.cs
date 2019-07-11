using System.Net.Http;
using System.Threading.Tasks;

namespace PartnerUser.Infrastructure.HttpClients
{
    public interface IBslApiClient
    {
        Task<HttpResponseMessage> SendAsync(
            string relativeUrl,
            HttpMethod httpMethod,
            HttpContent httpContent = null,
            string token = "");
    }
}
using Microsoft.AspNetCore.Http;

namespace Admin.Api.Extensions
{
    public static class HttpRequestHeadersExtensions
    {
        public static void Set(this IHeaderDictionary headers, string name, string value)
        {
            if (headers.ContainsKey(name))
            {
                headers.Remove(name);
            }
            headers.Add(name, value);
        }
    }
}

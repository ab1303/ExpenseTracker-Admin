using Microsoft.AspNetCore.Http;

namespace PartnerUser.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetFullUrl(this HttpContext context)
        {
            return context?.Request == null
                ? string.Empty
                : $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        }
    }
}

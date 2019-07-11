using System;
using Microsoft.AspNetCore.Http;
using PartnerUser.Api.Extensions;
using PartnerUser.Common;

namespace PartnerUser.Api.Middleware
{
    public abstract class MiddlewareBase
    {
        protected static string InitialiseCorrelationId(HttpContext context)
        {
            var correlationId = context.Request.Headers[Constants.CorrelationIdHeaderKey].ToString();

            if (!string.IsNullOrWhiteSpace(correlationId)) return correlationId;

            correlationId = Guid.NewGuid().ToString("D");
            SetCorrelationId(context, correlationId);

            return correlationId;
        }

        protected static void SetCorrelationId(HttpContext context, string correlationId)
        {
            context.Request.Headers.Set(Constants.CorrelationIdHeaderKey, correlationId);
        }
    }
}

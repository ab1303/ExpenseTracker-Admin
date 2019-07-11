using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using PartnerUser.Common;
using PartnerUser.Infrastructure.Logging.SerilogAdaptor;
using PartnerUser.Infrastructure.Logging.SerilogAdaptor.interfaces;

namespace PartnerUser.Api.Middleware.Logging
{
    public class LoggingMiddleware : MiddlewareBase
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdFieldName = "CorrelationId";

        private readonly Func<HttpContext, ILogEventEnricher> _correlationId = context =>
        {
            var correlationId = InitialiseCorrelationId(context);
            return new LogEventEnricher(CorrelationIdFieldName, correlationId);
        };

        private readonly Func<HttpContext, ILogEventEnricher> _ipEnricher = context =>
        {
            var ip = ReadIpFromXffHeader(context) ?? context.Connection.RemoteIpAddress?.ToString();
            return new LogEventEnricher("ClientIp", ip == "::1" ? "127.0.0.1" : ip);
        };

        private readonly Func<HttpContext, ILogEventEnricher> _httpVerb =
           ctx => new LogEventEnricher("HttpVerb", ctx.Request.Method);

        private readonly Func<string, ILogEventEnricher> _request =
            request => new LogEventEnricher("Request", request);

        private readonly Func<string, ILogEventEnricher> _response =
            response => new LogEventEnricher("Response", response);

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Serilog.ILogger logger, ILogContextService logContext)
        {
            try
            {
                //First, get the incoming request
                var formattedRequest = await FormatRequest(context.Request);

                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                string formattedResponse;

                //Create a new memory stream...
                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    context.Response.Body = responseBody;

                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);

                    //Format the response from the server
                    formattedResponse = await FormatResponse(context.Response);

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);
                }

                using (logContext.PushProperties(
                    _ipEnricher(context),
                    _correlationId(context),
                    _httpVerb(context),
                    _request(formattedRequest),
                    _response(formattedResponse)
                ))
                {
                    logger.Information("---- Logging Middleware of {applicationName} ---", Constants.ApplicationName);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw;
            }
        }
        private static string ReadIpFromXffHeader(HttpContext context)
        {
            var xForwardedForValue = context.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrWhiteSpace(xForwardedForValue))
                return null;
            return xForwardedForValue
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p));
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Seek(0, SeekOrigin.Begin);

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }
}

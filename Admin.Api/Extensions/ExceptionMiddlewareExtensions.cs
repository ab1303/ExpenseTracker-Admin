using System.Net;
using Admin.Common;
using Admin.Services.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Admin.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.Error($"Unhandled Error: {contextFeature.Error} in {Constants.ApplicationName}");

                        var serializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new Error
                        {
                            ErrorCode = ErrorCodes.UnknownError,
                            SystemMessage = "Uncaught Exception Handler Error"
                        }, serializerSettings));
                    }
                });
            });
        }
    }
}

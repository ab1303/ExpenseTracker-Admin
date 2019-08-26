using System.Collections.Generic;
using Admin.Common;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Admin.Api.SwashBuckle
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = Constants.AuthorizationHeaderName,
                In = "header",
                Description = "access token",
                Type = "string",
                Required = false
            });
        }
    }
}
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoilerPlate.Filters;

public class PreLoginHeaderOperationFilter() : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var path = context.ApiDescription.RelativePath;
        if (path != null && path.Contains("prelogin", StringComparison.OrdinalIgnoreCase))
        {

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ClientId",
                In = ParameterLocation.Header,
                Required = true, // Set to true if the header is required
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Client ID for pre-login."
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ClientSecret",
                In = ParameterLocation.Header,
                Required = true, // Set to true if the header is required
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Client Secret for pre-login."
            });
        }
    }
}

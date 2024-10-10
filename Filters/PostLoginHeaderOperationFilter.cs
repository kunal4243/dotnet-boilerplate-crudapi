using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoilerPlate.Filters;

public class PostLoginHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath != null && context.ApiDescription.RelativePath.Contains("postlogin", StringComparison.OrdinalIgnoreCase))
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "JWT-Token",
                In = ParameterLocation.Header,
                Required = true, // Set to true if the header is required
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Auth token for post-login."
            });
        }
    }
}

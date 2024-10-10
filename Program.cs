using BoilerPlate.Configurations;
using BoilerPlate.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Configuration.ConfigureLogging(builder);

Configuration.ConfigureServices(builder);

Configuration.ConfigureSwagger(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddlewareAsync>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.MapControllers();

await app.RunAsync();
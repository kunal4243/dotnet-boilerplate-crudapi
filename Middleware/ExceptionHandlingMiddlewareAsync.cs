using BoilerPlate.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;


namespace BoilerPlate.Middleware;
public class ExceptionHandlingMiddlewareAsync(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); // Proceed to the next middleware/request
        }
        catch (SecurityTokenValidationException) 
        {
            throw new CustomException(ErrorCode.AuthorizationFailed);
        }
        catch (SecurityTokenMalformedException)
        {
            throw new CustomException(ErrorCode.ValidationFailed);
        }
        catch (CustomException ex)  // Handle custom exceptions
        {
            await HandleCustomExceptionAsync(httpContext, ex);
        }
        catch (InternalCustomException ex)
        {
            await HandleInternalCustomExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)  // Handle unhandled exceptions
        { 
            await HandleGlobalExceptionAsync(httpContext, ex);
        }



    }


    private static Task HandleCustomExceptionAsync(HttpContext context, CustomException ex)
    {

        Log.Error(ex, "An exception occurred: ", ex.CustomMessage);

        context.Response.StatusCode = ex.ErrorCode switch
        {
            ErrorCode.Success => 200,
            ErrorCode.AuthorizationFailed => 401,
            ErrorCode.ValidationFailed =>400,
            ErrorCode.UserNotFound => 201,
            ErrorCode.InternalServerError =>500,
            _ => 500
            
        };
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(ex.CustomMessage, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        return context.Response.WriteAsync(result);
    }
    private static Task HandleInternalCustomExceptionAsync(HttpContext context, Exception ex)
    {
        Log.Error(ex, "A known exception occurred: {Message}", ex.Message);
        //_logger.LogError(ex, "Something went wrong.");  // Log the exception to a file or log service

        context.Response.StatusCode = 500;  // Return a generic internal server error status code
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(CommonResponse<string>.CreateResponse(ErrorCode.InternalServerError));
        return context.Response.WriteAsync(result);
    }
    private static Task HandleGlobalExceptionAsync(HttpContext context, Exception ex)
    {
        Log.Error(ex, "An exception occurred: {Message}", ex.Message);
        //_logger.LogError(ex, "Something went wrong.");  // Log the exception to a file or log service

        context.Response.StatusCode = 500;  // Return a generic internal server error status code
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(CommonResponse<string>.CreateResponse(ErrorCode.InternalServerError));
        return context.Response.WriteAsync(result);
    }
}

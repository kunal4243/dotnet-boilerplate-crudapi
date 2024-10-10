using BoilerPlate.Exceptions;
using BoilerPlate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BoilerPlate.Filters;

public class GlobalHttpRequestFilter(IConfiguration configuration, IAuthService authService) : IAsyncActionFilter
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthService _authService = authService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var request = context.HttpContext.Request;
        var path = request.Path.Value?.ToLower();

        if (path != null && (path.Contains("/prelogin") ))
        {
            if (  !request.Headers.TryGetValue("ClientId", out var clientId) || !request.Headers.TryGetValue("ClientSecret", out var clientSecret))
            {
                throw new CustomException(ErrorCode.ValidationFailed);
            }
            
            var isValidClient = await _authService.ValidateClientAsync(clientId.ToString(), clientSecret.ToString());

            if (isValidClient.Equals(false))
            {
                throw new CustomException(ErrorCode.AuthorizationFailed);
            }


        }
        
        else if (path != null && path.Contains("/postlogin"))
        {
            if (request.Headers.TryGetValue("JWT-Token", out var token))
            {
                // Extract the token string from the "Bearer {token}" format
                var tokenString = token.ToString().Split(" ")[^1];
                var _test = _configuration["ApplicationSettings:JWT_Secret"] ?? throw new InternalCustomException("secret key not found");
                // Validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_test)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Optional: eliminate default 5-minute clock skew
                };


                // Validate the token and set the claims principal
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, validationParameters, out _);
                context.HttpContext.User = claimsPrincipal; // Set the claims principal in the context

            }
            else
            {
                throw new CustomException(ErrorCode.ValidationFailed);
            }

        }
        else
        { context.Result = new ForbidResult(); }
        if (context.Result == null)
        {
            await next();
        }

    }

    public static async Task OnActionExecutedAsync(ActionExecutionDelegate next) => await next();

}

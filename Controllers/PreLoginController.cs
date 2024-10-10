using BoilerPlate.Data.DTO;
using BoilerPlate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PreLoginController(IAuthService authservice) : ControllerBase
{
    private readonly IAuthService _authService = authservice;

    /// <summary>
    /// Register new users.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    /// 
    ///     POST /PreLogin/register
    ///     
    ///     {
    ///         "userName": "string",
    ///         "password": "0oYGDcDRoNbTfA??ovdL",
    ///         "firstName": "string",
    ///         "middleName": "string",
    ///         "lastName": "string",
    ///         "country": "string",
    ///         "email": "user@example.com"
    ///     }
    /// 
    /// 
    /// Sample Response (Success):
    /// 
    ///     HTTP/1.1 200 OK
    ///     {
///             "errorCode": 0,
///             "errorMessage": "Success",
///             "data": {
///                 "userId": 4,
///                 "firstName": "RandomFirst",
///                 "middleName": "RandomMiddle",
///                 "lastName": "RandomLast",
///                 "country": "india",
///                 "email": "user@example.com"
///             }
///         }
/// 
/// Sample Response (Validation failed):
/// 
///     HTTP/1.1 400 ValidationFailed
///     {
///            "ErrorCode": 2,
///            "ErrorMessage": "Input validation failed.",
///            "Data": "null"
///        }
/// </remarks>
/// <response code="200">Success</response>
/// <response code="400">Validation failed. Try reformating the body.</response>
[HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterUserDto user)
    {
        var authUser = await _authService.Register(user);

        return Ok(authUser);
    }

    /// <summary>
    /// Login here using user id and password to get jwt-token
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <response code="201">Person created Successfully</response>
    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> Login(string userName, string password)
    {
        var token = await _authService.AuthenticateUser(userName, password);
        if (token == null)
        {
            return Unauthorized("Wrong Username or password");
        }
        return Ok(token);
    }


    /// <summary>
    /// Reset password using username and email if forgotten.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///  
    /// </remarks>
    /// <response code="201">Person created Successfully</response>

    [HttpGet]
    [Route("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string userName, string email)
    {
        var newPassword = await _authService.ForgotPassword(userName, email);
        return Ok(newPassword);
    }


}

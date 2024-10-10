using BoilerPlate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BoilerPlate.Controllers;


[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class PostLoginController(IUserService userservice) : ControllerBase
{
    private readonly IUserService _userService = userservice;


    /// <summary>
    /// Get users all users or individuals using id.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///     GET
    /// </remarks>
    /// <response code="201">Person created Successfully</response>
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync(int? id)
    {
        if (id == null)
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        int idNotNull = (id.Value);

        var user = await _userService.GetUserByIdAsync(idNotNull);

        return Ok(user);
    }


    /// <summary>
    /// Update trivial user details.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///     PUT
    /// </remarks>
    /// <response code="201">Person created Successfully</response>

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, string? firstName, string? middleName, string? lastName, string? country)
    {
        var User = await _userService.UpdateUserAsync(id, firstName, middleName, lastName, country);
        if (User == null)
        {
            return BadRequest("User Not Found");
        }
        return Ok(User);
    }


    /// <summary>
    /// Delete user by providing the id.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///     DELETE
    /// </remarks>
    /// <response code="201">Person created Successfully</response>

    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok("User successfully deleted");
    }






}




[ApiController]
[Route("api/Postlogin/[controller]")]
public class AuthChangeController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;



    /// <summary>
    /// Register new users.
    /// </summary>
    /// <remarks>
    /// Sample Request:
    ///     POST
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
    /// </remarks>
    /// <response code="201">Person created Successfully</response>

    [HttpPut]
    public async Task<IActionResult> UpdatePasswordOrStatusAsync(string? newPassword, string? newStatus, string oldPassword)
    {
        await _authService.ChangePasswordOrStatus(oldPassword, newPassword, User, newStatus);
        return Ok($"The password or status of {User.FindFirst(ClaimTypes.Name)?.Value} have been successfully updated");
    }




}

using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using BoilerPlate.Exceptions;
using System.Security.Claims;

namespace BoilerPlate.Services.Interfaces;

public interface IAuthService
{
    Task<CommonResponse<string?>> AuthenticateUser(string username, string password);

    Task<CommonResponse<UserDetail>> Register(RegisterUserDto regUser);

    Task<CommonResponse<AuthUser>> ChangePasswordOrStatus(string oldPassword, string? newPassword, ClaimsPrincipal userClaim, string? newStatus);

    Task<CommonResponse<string>> ForgotPassword(string userName, string? email);

    Task TerminateUserAsync(int id);

    Task<bool> ValidateClientAsync(string clientId, string clientSecret);
}

using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using BoilerPlate.Exceptions;
using BoilerPlate.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using UserStatus = BoilerPlate.Data.DTO.UserStatus;

namespace BoilerPlate.Services.Implementations;

public class AuthService(IConfiguration configuration, IAuthUsersDao authUserDao, IUserDetailsDao userDetailsDao, IClientAuthDao clientAuthDao) : IAuthService
{
    private readonly IAuthUsersDao _authUserDao = authUserDao;
    private readonly IUserDetailsDao _userDetailsDao = userDetailsDao;
    private readonly IConfiguration _configuration = configuration;
    private readonly IClientAuthDao _clientAuthDao = clientAuthDao;

    public async Task<CommonResponse<string?>> AuthenticateUser(string username, string password)
    {
        if (username.Length < 3 || username.Length > 20 || password.Length < 8 || password.Length > 20)        
            throw new CustomException(ErrorCode.ValidationFailed);
        
        var authUser = await _authUserDao.GetUserByUserName(username) ?? throw new CustomException(ErrorCode.UserNotFound);
        if (authUser.UserStatus == Data.Entities.UserStatus.Terminated) throw new CustomException(ErrorCode.UserNotFound);
        bool isValidUser = PasswordService.VerifyPassword(password, authUser.PasswordHash);
        if (!isValidUser)        
            throw new CustomException(ErrorCode.AuthorizationFailed);
        

        var user = await _userDetailsDao.GetUserByIdAsync(authUser.UserId) ?? throw new CustomException(ErrorCode.UserNotFound);
        var token = GenerateJWTToken(user, authUser);
        return CommonResponse<string?>.CreateResponse(ErrorCode.Success, token);

    }




    private string GenerateJWTToken(UserDetail user, AuthUser authUser)
    {
        var claims = new List<Claim>{
            new (ClaimTypes.Email ,user.Email),
            new (ClaimTypes.Name,authUser.UserName)
        };
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(5),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes((_configuration["ApplicationSettings:JWT_Secret"] ?? throw new InternalCustomException("Secret key not found", 500)))
                    ),
                SecurityAlgorithms.HmacSha256Signature
            )
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public async Task<CommonResponse<UserDetail>> Register(RegisterUserDto regUser)
    {
        if (await _userDetailsDao.GetUserByEmail(regUser.Email) != null)
            throw new CustomException(ErrorCode.ValidationFailed);

        if (await _authUserDao.GetUserByUserName(regUser.UserName) != null)
            throw new CustomException(ErrorCode.ValidationFailed);

        if (regUser.FirstName != null && (regUser.FirstName.Length < 3 || regUser.FirstName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (regUser.MiddleName != null && (regUser.MiddleName.Length < 3 || regUser.MiddleName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (regUser.LastName != null && (regUser.LastName.Length < 3 || regUser.LastName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (regUser.Country != null && (regUser.Country.Length < 3 || regUser.Country.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        var authUserDto = new AddAuthUserDto()
        {
            UserName = regUser.UserName,
            PasswordHash = PasswordService.HashPassword(regUser.Password)
        };

        var user = new AddUserDto()
        {
            Email = regUser.Email,
            FirstName = regUser.FirstName,
            MiddleName = regUser.MiddleName,
            LastName = regUser.LastName,
            Country = regUser.Country
        };

        _ = await _authUserDao.AddUserAsync(authUserDto);


        var User = await _userDetailsDao.AddUserAsync(user);
        return CommonResponse<UserDetail>.CreateResponse(ErrorCode.Success, User);

    }

    public async Task<CommonResponse<AuthUser>> ChangePasswordOrStatus(string oldPassword, string? newPassword, ClaimsPrincipal userClaim, string? newStatus)
    {
        var userName = userClaim.FindFirst(ClaimTypes.Name)?.Value ?? throw new CustomException(ErrorCode.AuthorizationFailed);

        if (newStatus == null && newPassword == null) throw new CustomException(ErrorCode.ValidationFailed);

        if (newPassword != null && (newPassword.Length < 8 || newPassword.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        var regex = new Regex(@"^[a-zA-Z0-9\-\?\$\%]+$");
        if (newPassword != null && !regex.IsMatch(newPassword)) throw new CustomException(ErrorCode.ValidationFailed);

        if (newStatus != null && newStatus.ToLower() != "terminated" && newStatus.ToLower() != "active" && newStatus.ToLower() != "inactive") { throw new CustomException(ErrorCode.ValidationFailed); }
        AuthUser user = await _authUserDao.GetUserByUserName(userName) ?? throw new CustomException(ErrorCode.UserNotFound);
        if (!PasswordService.VerifyPassword(oldPassword, user.PasswordHash)) throw new CustomException(ErrorCode.AuthorizationFailed);
        Enum.TryParse(newStatus, true, out UserStatus status);

        UpdateAuthUserDto authUserDto = new()
        {
            UserId = user.UserId,
            PasswordHash = PasswordService.HashPassword(newPassword ?? oldPassword),
            UserStatus = status
        };

        var User = await _authUserDao.UpdateUserAsync(authUserDto) ?? throw new CustomException(ErrorCode.UserNotFound);
        return CommonResponse<AuthUser>.CreateResponse(ErrorCode.Success, User); 
    }

    public async Task<CommonResponse<string>> ForgotPassword(string userName, string? email)
    {
        var authUser = await _authUserDao.GetUserByUserName(userName) ?? throw new CustomException(ErrorCode.UserNotFound);

        var user = await _userDetailsDao.GetUserByIdAsync(authUser.UserId) ?? throw new CustomException(ErrorCode.UserNotFound);

        if (user.Email != email)
            throw new CustomException(ErrorCode.ValidationFailed);
        var password = PasswordService.GenerateRandomPassword(10);
        UpdateAuthUserDto authUserDto = new()
        {
            UserId = user.UserId,
            PasswordHash = PasswordService.HashPassword(password),
        };

        _ = await _authUserDao.UpdateUserAsync(authUserDto) ?? throw new CustomException(ErrorCode.UserNotFound);
        return CommonResponse<string>.CreateResponse(ErrorCode.Success, password);
    }

    public async Task TerminateUserAsync(int id)
    {
        _ = await _authUserDao.GetUserByIdAsync(id) ?? throw new CustomException(ErrorCode.UserNotFound);
        UpdateAuthUserDto updateUser = new()
        {
            UserId = id,
            UserStatus = UserStatus.Terminated
        };
        _ = await _authUserDao.UpdateUserAsync(updateUser);
        return;
    }

    public async Task<bool> ValidateClientAsync(string clientId, string clientSecret)
    {
        var client = await _clientAuthDao.GetUserByClientIdAsync(clientId);
        if (client == null) return false;
        if (!client.ClientSecret.Equals(clientSecret)) { return false; }

        if (client.Expiry < DateTime.UtcNow) return false;
        return true;
    }

}

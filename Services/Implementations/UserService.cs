using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using BoilerPlate.Exceptions;

using BoilerPlate.Services.Interfaces;
using UserStatus = BoilerPlate.Data.Entities.UserStatus;

namespace BoilerPlate.Services.Implementations;

public class UserService(IUserDetailsDao userDetailDao, IAuthUsersDao authUsersDao, IAuthService authService) : IUserService
{
    private readonly IUserDetailsDao _userDetailDao = userDetailDao;
    private readonly IAuthUsersDao _authUsersDao = authUsersDao;
    private readonly IAuthService _authService = authService;

    public async Task<CommonResponse<UserDetail>> GetUserByIdAsync(int id)
    {
        if (id <= 0) throw new CustomException(ErrorCode.ValidationFailed);
        var userAuth = await _authUsersDao.GetUserByIdAsync(id) ?? throw new CustomException(ErrorCode.UserNotFound);
        if (userAuth.UserStatus == UserStatus.Terminated) throw new CustomException(ErrorCode.UserNotFound);
        var user = await _userDetailDao.GetUserByIdAsync(id) ?? throw new CustomException(ErrorCode.UserNotFound);

        return CommonResponse<UserDetail>.CreateResponse(ErrorCode.Success,user);
    }

    public async Task<CommonResponse<List<UserDetail>>> GetAllUsersAsync()
    {

        var users = await _userDetailDao.GetAllUsersAsync();
        if (users.Count == 0) throw new CustomException(ErrorCode.UserNotFound);
        return CommonResponse<List<UserDetail>>.CreateResponse(ErrorCode.Success, users);

    }

    public async Task<CommonResponse<UserDetail?>> AddUserAsync(AddUserDto user)
    {
        var existingUser = await _userDetailDao.GetUserByEmail(user.Email);
        if (existingUser != null) throw new CustomException(ErrorCode.ValidationFailed);

        var User = await _userDetailDao.AddUserAsync(user);
        return CommonResponse<UserDetail?>.CreateResponse(ErrorCode.Success, User);
    }

    public async Task<CommonResponse<UserDetail?>> UpdateUserAsync(int id, string? firstName, string? middleName, string? lastName, string? country)
    {
        if (id <= 0) throw new CustomException(ErrorCode.ValidationFailed);


        if (firstName != null && (firstName.Length < 3 || firstName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (middleName != null && (middleName.Length < 3 || middleName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (lastName != null && (lastName.Length < 3 || lastName.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        if (country != null && (country.Length < 3 || country.Length > 20)) throw new CustomException(ErrorCode.ValidationFailed);

        var authUser = await _authUsersDao.GetUserByIdAsync(id) ?? throw new CustomException(ErrorCode.UserNotFound);
        if (authUser.UserStatus == UserStatus.Terminated) throw new CustomException(ErrorCode.UserNotFound);

        UpdateUserDto user = new()
        {
            UserId = id,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            Country = country
        };
        var User = await _userDetailDao.UpdateUserAsync(user) ?? throw new CustomException(ErrorCode.UserNotFound); 
        return CommonResponse<UserDetail?>.CreateResponse(ErrorCode.Success, User);
               

    }

    public async Task DeleteUserAsync(int id)
    {
        if (id <= 0) throw new CustomException(ErrorCode.ValidationFailed);
        _ = await _userDetailDao.GetUserByIdAsync(id) ?? throw new CustomException(ErrorCode.UserNotFound);
        await _authService.TerminateUserAsync(id);
    }

}

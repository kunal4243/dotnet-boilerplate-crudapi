using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using BoilerPlate.Exceptions;

namespace BoilerPlate.Services.Interfaces
{
    public interface IUserService
    {
        Task<CommonResponse<UserDetail>> GetUserByIdAsync(int id);
        Task<CommonResponse<List<UserDetail>>> GetAllUsersAsync();
        Task<CommonResponse<UserDetail?>> AddUserAsync(AddUserDto user);
        Task<CommonResponse<UserDetail?>> UpdateUserAsync(int id, string? firstName, string? middleName, string? lastName, string? country);
        Task DeleteUserAsync(int id);
    }
}

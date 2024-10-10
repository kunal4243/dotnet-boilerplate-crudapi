using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;

namespace BoilerPlate.Data.DAO.Interface;
/// <summary>
/// 
/// </summary>
public interface IAuthUsersDao
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<List<AuthUser>> GetAllUsersAsync();

    public Task<AuthUser?> GetUserByIdAsync(int id);

    public Task<AuthUser?> GetUserByUserName(string username);

    public Task<AuthUser?> UpdateUserAsync(UpdateAuthUserDto user);

    public Task<AuthUser> AddUserAsync(AddAuthUserDto user);

    public Task<bool> DeleteUserAsync(int id);
}

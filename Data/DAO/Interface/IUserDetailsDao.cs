using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;

namespace BoilerPlate.Data.DAO.Interface;

public interface IUserDetailsDao
{
    public Task<List<UserDetail>> GetAllUsersAsync();

    public Task<UserDetail?> GetUserByIdAsync(int id);

    public Task<UserDetail?> GetUserByEmail(string email);

    public Task<UserDetail?> UpdateUserAsync(UpdateUserDto user);

    public Task<UserDetail> AddUserAsync(AddUserDto user);

    public Task DeleteUserAsync(int id);
}

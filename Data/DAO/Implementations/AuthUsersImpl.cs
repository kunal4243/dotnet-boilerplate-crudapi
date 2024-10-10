using BoilerPlate.Data.Context;
using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Data.DAO.Implementations;

/// <summary>
/// 
/// </summary>
/// <param name="dbContext"></param>
public class AuthUsersImpl(ApplicationDbContext dbContext) : IAuthUsersDao
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<AuthUser>> GetAllUsersAsync()
    {
        var Users = await _dbContext.AuthUsers.ToListAsync();
        return Users;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<AuthUser?> GetUserByIdAsync(int id)
    {
        var User = await _dbContext.AuthUsers.FindAsync(id);
        return User;
    }

    public async Task<AuthUser?> GetUserByUserName(string username)
    {
        var User = await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.UserName == username);
        return User;

    }

    public async Task<AuthUser?> UpdateUserAsync(UpdateAuthUserDto user)
    {
        var User = await _dbContext.AuthUsers.FindAsync(user.UserId);
        if (User == null) return User;

        foreach (var property in user.GetType().GetProperties())
        {
            if (property.GetValue(user) == null) continue;
            var targetProperty = User.GetType().GetProperty(property.Name);
            if (targetProperty != null && targetProperty.CanWrite)
            {
                var value = property.GetValue(user);
                targetProperty.SetValue(User, value);
            }
        }

        _dbContext.AuthUsers.Update(User);
        await _dbContext.SaveChangesAsync();
        return User;
    }

    public async Task<AuthUser> AddUserAsync(AddAuthUserDto user)
    {
        var User = new AuthUser()
        {
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            RefreshToken = user.RefreshToken
        };

        await _dbContext.AuthUsers.AddAsync(User);
        await _dbContext.SaveChangesAsync();

        return User;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _dbContext.AuthUsers.FindAsync(id);

        if (user == null) return false;

        _ = _dbContext.AuthUsers.Remove(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }

}

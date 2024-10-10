using BoilerPlate.Data.Context;
using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Data.DTO;
using BoilerPlate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Data.DAO.Implementations;


public class UserDetailsImpl(ApplicationDbContext dbContext) : IUserDetailsDao
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<UserDetail>> GetAllUsersAsync()
    {
        var Users = await _dbContext.UserDetails.ToListAsync();
        return Users;
    }

    public async Task<UserDetail?> GetUserByIdAsync(int id)
    {
        var User = await _dbContext.UserDetails.FindAsync(id);
        return User;
    }

    public async Task<UserDetail?> GetUserByEmail(string email)
    {
        var User = await _dbContext.UserDetails.Where(u => u.Email == email).FirstOrDefaultAsync();
        return User;
    }

    public async Task<UserDetail?> UpdateUserAsync(UpdateUserDto user)
    {
        var User = await _dbContext.UserDetails.FindAsync(user.UserId);
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

        _dbContext.UserDetails.Update(User);
        await _dbContext.SaveChangesAsync();
        return User;

    }

    public async Task<UserDetail> AddUserAsync(AddUserDto user)
    {
        var User = new UserDetail()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MiddleName = user.MiddleName,
            Country = user.Country
        };

        await _dbContext.UserDetails.AddAsync(User);
        await _dbContext.SaveChangesAsync();

        return User;



    }

    public async Task DeleteUserAsync(int id)
    {
        var User = await _dbContext.UserDetails.FindAsync(id);

        if (User != null) _dbContext.UserDetails.Remove(User);
        await _dbContext.SaveChangesAsync();
    }
}

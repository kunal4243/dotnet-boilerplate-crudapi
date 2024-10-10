using BoilerPlate.Data.Context;
using BoilerPlate.Data.DAO.Interface;
using BoilerPlate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate.Data.DAO.Implementations;

public class ClientAuthImpl(ApplicationDbContext dbContext) : IClientAuthDao
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<List<Client>> GetAllUsersAsync()
    {
        var clients = await _dbContext.Clients.ToListAsync();
        return clients;

    }

    public async Task<Client?> GetUserByClientIdAsync(string clientId)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(u => u.ClientId == clientId);
        return client;
    }

    public async Task<Client?> AddUserAsync(string clientId, string clientSecret, int forDays)
    {
        var client = new Client()
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Expiry = DateTime.UtcNow.AddDays(forDays)
        };
        _ = await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
        return client;

    }

    public async Task<bool> DeleteClientAsync(string clientId)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(u => u.ClientId == clientId);
        if (client == null) { return false; }
        _ = _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();
        return true;

    }

}

using BoilerPlate.Data.Entities;

namespace BoilerPlate.Data.DAO.Interface
{
    public interface IClientAuthDao
    {
        public Task<List<Client>> GetAllUsersAsync();

        public Task<Client?> GetUserByClientIdAsync(string clientId);

        public Task<Client?> AddUserAsync(string clientId, string clientSecret, int forDays);

        public Task<bool> DeleteClientAsync(string clientId);

    }
}

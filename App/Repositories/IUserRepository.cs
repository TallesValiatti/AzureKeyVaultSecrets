using App.Entities;

namespace App.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetAsync(string id);
        Task CreateAsync(User user);
    }
}
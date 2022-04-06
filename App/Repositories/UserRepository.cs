using App.Entities;
using MongoDB.Driver;

namespace App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string _collectionName = "user";
        private readonly IMongoCollection<User> _userCollection;  

        public UserRepository(DbContext context)
        {
            _userCollection = context.Database.GetCollection<User>(_collectionName);
        }
        public async Task<IEnumerable<User>> GetAllAsync() =>
            (await _userCollection.FindAsync(_ => true)).ToList();

        public async Task<User?> GetAsync(string id) =>
            await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _userCollection.InsertOneAsync(user);    
    }
}

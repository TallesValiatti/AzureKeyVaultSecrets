using MongoDB.Driver;

namespace App.Repositories
{
    public class DbContext
    {
        private const string _databaseName = "db-user-service";
        public IMongoDatabase Database
        {
            get { return _database; }
        }
        
        private readonly IMongoDatabase _database;

        public DbContext(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["cosmos-user-service-connection-string"]);
            _database = mongoClient.GetDatabase(_databaseName);
        }
    }
}
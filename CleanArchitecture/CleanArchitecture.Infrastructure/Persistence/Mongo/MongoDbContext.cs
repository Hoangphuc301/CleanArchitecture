using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CleanArchitecture.Infrastructure.Persistence.Mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;

            var client = new MongoClient(settings.ConnectionString);

            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<MenuLog> MenuLogs => _database.GetCollection<MenuLog>("MenuLogs");
        public IMongoCollection<NewLog> NewLogs => _database.GetCollection<NewLog>("NewLogs");
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Test_DB.Models
{
    public class PostContext
    {
        private readonly IMongoDatabase _database = null;
        
        public PostContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Post> Posts
        {
            get
            {
                return _database.GetCollection<Post>("Posts");
            }
        }
    }
}
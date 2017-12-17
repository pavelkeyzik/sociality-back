using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Test_DB.Models
{
    public class MessageContext
    {
        private readonly IMongoDatabase _database = null;
        
        public MessageContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Message> Messages
        {
            get
            {
                return _database.GetCollection<Message>("Messages");
            }
        }
    }
}
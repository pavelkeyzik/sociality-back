using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Test_DB.Models
{
    public class DialogContext
    {
        private readonly IMongoDatabase _database = null;
        
        public DialogContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Dialog> Dialogs
        {
            get
            {
                return _database.GetCollection<Dialog>("Dialogs");
            }
        }
    }
}
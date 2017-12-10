using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Test_DB.Models
{
    public class ImageContext
    {
        private readonly IMongoDatabase _database = null;

        public ImageContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Image> Images
        {
            get
            {
                return _database.GetCollection<Image>("Images");
            }
        }
    }
}
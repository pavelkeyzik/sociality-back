using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("login")]
        public string Login { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
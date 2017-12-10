using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("login")]
        public string Login { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("role")]
        public string Role { get; set; }
        [BsonElement("avatar")]
        public string Avatar { get; set; }
    }
}
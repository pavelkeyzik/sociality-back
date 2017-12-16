using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class Profile
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("login")]
        public string Login { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("online")]
        public bool Online { get; set; }
        
        [BsonElement("phone")]
        public string Phone { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; }
        
        [BsonElement("avatar")]
        public string Avatar { get; set; }
        
        [BsonElement("gender")]
        public string Gender { get; set; }
    }
}
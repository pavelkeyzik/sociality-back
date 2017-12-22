using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("meId")]
        public string MeId { get; set; }
        
        [BsonElement("text")]
        public string Text { get; set; }
        
        [BsonElement("image")]
        public string Image { get; set; }
        
        [BsonElement("likes")]
        public int Likes { get; set; }
    }
}
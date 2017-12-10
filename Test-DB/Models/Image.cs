using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class Image
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("url")]
        public string Url { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
    }
}
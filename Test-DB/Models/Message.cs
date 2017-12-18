using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("authorId")]
        public string AuthorId { get; set; }
        
        [BsonElement("recipientId")]
        public string RecipientId { get; set; }
        
        [BsonElement("friendId")]
        public string FriendId { get; set; }
        
        [BsonElement("type")]
        public string Type { get; set; }
        
        [BsonElement("messageText")]
        public string MessageText { get; set; }
        
        [BsonElement("messageImage")]
        public string MessageImage { get; set; }
    }
}
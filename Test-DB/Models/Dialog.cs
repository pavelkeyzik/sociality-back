using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test_DB.Models
{
    public class Dialog
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("meId")]
        public string MeId { get; set; }
        
        [BsonElement("friendId")]
        public string friendId { get; set; }

        [BsonElement("lastMessage")]
        public string LastMessage { get; set; }
        
        [BsonElement("dateMessage")]
        public DateTime DateMessage { get; set; }
        
        [BsonElement("unreaded")]
        public int Unreaded { get; set; }
        
    }
}
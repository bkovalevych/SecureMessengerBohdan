using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SecureMessengerBohdan.Application.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid FromId { get; set; }

        public string FromName { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChatId { get; set; }

        public string ChatName { get; set; }

        public string Text { get; set; }

        public bool IsUnread { get; set; }

        public DateTimeOffset Sent { get; set; }
    }
}

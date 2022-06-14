using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SecureMessengerBohdan.Security.Models
{
    public class ChatKey
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChatId { get; set; }

        public byte[] IV { get; set; }

        public byte[] Key { get; set; }
    }
}

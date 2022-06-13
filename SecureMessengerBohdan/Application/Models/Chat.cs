using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Application.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Name { get; set; }

        public List<ApplicationUser> Members { get; set; } = new List<ApplicationUser>();

        
        public List<Message> Messages { get; set; } = new List<Message>();

        public DateTimeOffset Created { get; set; }
    }
}

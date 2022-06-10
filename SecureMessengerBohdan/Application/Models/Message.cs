using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureMessengerBohdan.Application.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid FromId { get; set; }

        public string FromName { get; set; }

        public Guid ToId { get; set; }

        public string ToName { get; set; }

        public string Text { get; set; }

        public bool IsUnread { get; set; }
    }
}

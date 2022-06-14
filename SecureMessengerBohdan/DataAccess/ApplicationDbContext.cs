using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;

namespace SecureMessengerBohdan.DataAccess
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        public ApplicationDbContext(IOptions<MongoDbConfig> artWorkSettings)
        {
            var mongoClient = new MongoClient(
            artWorkSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(
                artWorkSettings.Value.Name);
            var index = Builders<Message>.IndexKeys
                .Ascending(m => m.ChatId)
                .Descending(m => m.Sent);
            MessageRecord.Indexes
                .CreateOne(new CreateIndexModel<Message>(index));
        }

        public IMongoCollection<Message> MessageRecord
        {
            get
            {
                return _mongoDatabase.GetCollection<Message>(nameof(MessageRecord));
            }
        }

        public IMongoCollection<Chat> ChatRecord
        {
            get
            {
                return _mongoDatabase.GetCollection<Chat>(nameof(ChatRecord));
            }
        }
    }
}

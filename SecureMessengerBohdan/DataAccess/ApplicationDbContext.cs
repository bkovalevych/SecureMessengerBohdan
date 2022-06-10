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
        }

        public IMongoCollection<Message> MessageRecord
        {
            get
            {
                return _mongoDatabase.GetCollection<Message>(nameof(MessageRecord));
            }
        }
    }
}

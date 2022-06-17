using MediatR;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Security.Models;

namespace SecureMessengerBohdan.Security.Requests.CreateChatKeys
{
    public class SaveEncryptedChatKeysHandler : IRequestHandler<SaveEncryptedChatKeysRequest>
    {
        private readonly ApplicationDbContext _context;

        public SaveEncryptedChatKeysHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveEncryptedChatKeysRequest request, CancellationToken cancellationToken)
        {
            var chatKeys = request.ChatKeys.Select(it => new ChatKey()
            {
                ChatId = it.ChatId,
                IV = Convert.FromBase64String(it.IV),
                Key = Convert.FromBase64String(it.Key),
                UserId = it.UserId
            });
            await _context.ChatKeyRecord.InsertManyAsync(chatKeys);
            return Unit.Value;
        }
    }
}

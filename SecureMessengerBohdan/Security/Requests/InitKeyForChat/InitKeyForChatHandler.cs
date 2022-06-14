using MediatR;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Security.Models;
using System.Security.Cryptography;

namespace SecureMessengerBohdan.Security.Requests.InitKeyForChat
{
    public class InitKeyForChatHandler : IRequestHandler<InitKeyForChatRequest>
    {
        private readonly ApplicationDbContext _context;

        public InitKeyForChatHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(InitKeyForChatRequest request, CancellationToken cancellationToken)
        {
            using var myAes = Aes.Create();
            var chatKey = new ChatKey()
            {
                ChatId = request.ChatId,
                IV = myAes.IV,
                Key = myAes.Key
            };
            await _context.ChatKeyRecord.InsertOneAsync(chatKey);

            return Unit.Value; 
        }
    }
}

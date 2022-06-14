using MediatR;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Exceptions;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Models;
using SecureMessengerBohdan.Security.Models;
using System.Security.Cryptography;

namespace SecureMessengerBohdan.Security.Requests.GetChatKey
{
    public class GetChatKeyHandler : IRequestHandler<GetChatKeyRequest, GetChatKeyDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUser;
        private byte[] _exponent = { 1, 0, 1 };
        public GetChatKeyHandler(ApplicationDbContext context, CurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<GetChatKeyDto> Handle(GetChatKeyRequest request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _currentUser.GetUser();
            CheckMember(user);
            var filter = Builders<ChatKey>.Filter.Eq(it => it.ChatId, request.ChatId);
            var chatKey = await _context.ChatKeyRecord.Find(filter)
                .FirstOrDefaultAsync(cancellationToken);
            CheckKey(chatKey);

            var encryptedResult = EncryptKeys(
                request.ChatId,
                chatKey.Key,
                chatKey.IV,
                request.PublicRSAKey);

            return encryptedResult;
        }

        private GetChatKeyDto EncryptKeys(string chatId, byte[] key, byte[] iv, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);
            byte[] rawEncryptedKey = rsa.Encrypt(key, RSAEncryptionPadding.Pkcs1);
            byte[] rawEncryptedIV = rsa.Encrypt(iv, RSAEncryptionPadding.Pkcs1);

            string encryptedKey = Convert.ToBase64String(rawEncryptedKey);
            string encryptedIV = Convert.ToBase64String(rawEncryptedIV);

            return new GetChatKeyDto()
            {
                ChatId = chatId,
                IV = encryptedIV,
                Key = encryptedKey
            };
        }

        private void CheckMember(ApplicationUser user)
        {
            var isMember = _context.ChatRecord.AsQueryable()
                .Any(chat =>
                chat.Members.Any(member => member.Id == user.Id));
            if (!isMember)
            {
                throw new DomainException("user is not a member of a group");
            }
        }
        private void CheckKey(ChatKey key)
        {
            if (key == null)
            {
                throw new DomainException("key not found");
            }
        }
    }
}

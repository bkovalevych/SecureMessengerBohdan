﻿using MediatR;
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
            var filter = Builders<ChatKey>.Filter;
            var condition = filter.And(
                filter.Eq(it => it.UserId, user.Id.ToString()),  
                filter.Eq(it => it.ChatId, request.ChatId));
            var chatKey = await _context.ChatKeyRecord.Find(condition)
                .FirstOrDefaultAsync(cancellationToken);
            CheckKey(chatKey);

            var encryptedResult = new GetChatKeyDto()
            {
                ChatId = request.ChatId,
                Key = Convert.ToBase64String(chatKey.Key),
                IV = Convert.ToBase64String(chatKey.IV),
                UserId = user.Id.ToString()
            };

            return encryptedResult;
        }

        private void CheckMember(ApplicationUser user)
        {
            var isMember = _context.ChatRecord.AsQueryable()
                .Any(chat =>
                chat.Members.Any(member => member == user.Id.ToString()));
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

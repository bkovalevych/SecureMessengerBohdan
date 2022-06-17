using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.Requests.CreateChat;
using SecureMessengerBohdan.Application.Requests.GetChats;
using SecureMessengerBohdan.Application.Requests.GetMessages;
using SecureMessengerBohdan.Application.Requests.InitChats;
using SecureMessengerBohdan.Application.Wrappers;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Hubs;
using SecureMessengerBohdan.Hubs.Interfaces;
using SecureMessengerBohdan.Security.Requests.CreateChatKeys;
using SecureMessengerBohdan.Security.Requests.GetChatKey;
using System.Security.Claims;

namespace SecureMessengerBohdan.Controllers
{
    [Authorize]
    public class ChatsController : BaseController
    {
        private readonly IHubContext<ChatsHub, IChatsHubClient> _chatsHub;
        private readonly ApplicationDbContext _context;

        public ChatsController(ISender sender, 
            IHubContext<ChatsHub, IChatsHubClient> chatsHub,
            ApplicationDbContext context) : base(sender)
        {
            _chatsHub = chatsHub;
            _context = context;
        }

        [HttpGet]
        public async Task<List<GetChatDto>> GetChats(CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetChatsRequest(), cancellationToken);
        }

        [HttpGet("{id}/messages")]
        public async Task<PagingList<GetMessageDto>> GetMessages(string id, [FromQuery] Paging paging, CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetMessagesRequest()
            {
                ChatId = id,
                Paging = paging
            }, cancellationToken);
        }

        [HttpPost("init")]
        public async Task<List<Chat>> InitChats(CancellationToken cancellationToken)
        {
            return await Sender.Send(new InitChatsRequest(), cancellationToken);
        }

        [HttpPost("key")]
        public async Task<GetChatKeyDto> GetChatKey(GetChatKeyRequest request, CancellationToken cancellationToken)
        {
            return await Sender.Send(request, cancellationToken);
        }

        [HttpPost("saveEncryptedChatKeys")]
        public async Task CreateChatKeys(List<GetChatKeyDto> chatKeys, CancellationToken cancellationToken)
        {
            await Sender.Send(new SaveEncryptedChatKeysRequest()
            {
                ChatKeys = chatKeys
            }, cancellationToken);
            foreach(var grouping in chatKeys.GroupBy(chat => chat.ChatId)) {
                var chat = _context
                    .ChatRecord
                    .AsQueryable()
                    .First(it => it.Id == grouping.Key);
                var chatDto = new GetChatDto()
                {
                    Id = chat.Id,
                    Name = chat.Name
                };

                await _chatsHub.Clients
                    .Users(chatKeys.Select(it => it.UserId))
                    .ChatCreated(chatDto);
            }
        }

        [HttpPost]
        public async Task<GetChatDto> CreateChat(CreateChatRequest request, CancellationToken cancellationToken)
        {
            request.MemberIds.Add(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var chat = await Sender.Send(request, cancellationToken);
            
            return chat;
        }
    }
}

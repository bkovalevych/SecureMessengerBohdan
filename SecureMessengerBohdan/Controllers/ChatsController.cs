using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.Requests.CreateChat;
using SecureMessengerBohdan.Application.Requests.GetChats;
using SecureMessengerBohdan.Application.Requests.GetMessages;
using SecureMessengerBohdan.Application.Requests.InitChats;
using SecureMessengerBohdan.Application.Wrappers;
using SecureMessengerBohdan.Hubs;
using SecureMessengerBohdan.Hubs.Interfaces;
using SecureMessengerBohdan.Security.Requests.GetChatKey;
using SecureMessengerBohdan.Security.Requests.InitKeyForChat;
using System.Security.Claims;

namespace SecureMessengerBohdan.Controllers
{
    [Authorize]
    public class ChatsController : BaseController
    {
        private readonly IHubContext<ChatsHub, IChatsHubClient> _chatsHub;

        public ChatsController(ISender sender, IHubContext<ChatsHub, IChatsHubClient> chatsHub) : base(sender)
        {
            _chatsHub = chatsHub;
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
        public async Task InitChats(CancellationToken cancellationToken)
        {
            await Sender.Send(new InitChatsRequest(), cancellationToken);
        }

        [HttpPost("key")]
        public async Task<GetChatKeyDto> GetChatKey(GetChatKeyRequest request, CancellationToken cancellationToken)
        {
            return await Sender.Send(request, cancellationToken);
        }

        [HttpPost]
        public async Task<GetChatDto> CreateChat(CreateChatRequest request, CancellationToken cancellationToken)
        {
            request.MemberIds.Add(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var chat = await Sender.Send(request, cancellationToken);
            await Sender.Send(new InitKeyForChatRequest() { ChatId= chat.Id }, cancellationToken);
            await _chatsHub.Clients
                .Users(request.MemberIds)
                .ChatCreated(chat);
            return chat;
        }
    }
}

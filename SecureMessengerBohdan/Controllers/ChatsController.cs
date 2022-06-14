using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.Requests.GetChats;
using SecureMessengerBohdan.Application.Requests.GetMessages;
using SecureMessengerBohdan.Application.Requests.InitChats;
using SecureMessengerBohdan.Application.Wrappers;
using SecureMessengerBohdan.Security.Requests.GetChatKey;

namespace SecureMessengerBohdan.Controllers
{
    [Authorize]
    public class ChatsController : BaseController
    {
        public ChatsController(ISender sender) : base(sender)
        {
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
    }
}

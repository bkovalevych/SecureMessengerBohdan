using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SecureMessengerBohdan.Application.Requests.SendMessage;
using SecureMessengerBohdan.Application.Services;
using System.Security.Claims;

namespace SecureMessengerBohdan.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ISender _sender;
        private readonly CurrentUserService _currentUserService;

        public ChatHub(ISender sender, CurrentUserService currentUserService)
        {
            _sender = sender;
            _currentUserService = currentUserService;
        }

        public override async Task OnConnectedAsync()
        {
            var id = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, id);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var id = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, id);
            }
        }

        public async Task ActivateChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task DeactivateChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendMessage(SendMessageRequest message)
        {
            _currentUserService.User = Context.User;
            var messageDto = await _sender.Send(message);
            
            await Clients.Group(message.ChatId)
                .SendAsync("ReceiveMessage", messageDto);
        }
    }
}

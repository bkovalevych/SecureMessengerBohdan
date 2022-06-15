using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SecureMessengerBohdan.Application.Requests.SendMessage;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.Hubs.Interfaces;

namespace SecureMessengerBohdan.Hubs
{
    [Authorize]
    public class MessagesHub : Hub<IMessagesHubClient>
    {
        private readonly ISender _sender;
        private readonly CurrentUserService _currentUserService;

        public MessagesHub(ISender sender, CurrentUserService currentUserService)
        {
            _sender = sender;
            _currentUserService = currentUserService;
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
                .ReceiveMessage(messageDto);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SecureMessengerBohdan.Hubs.Interfaces;

namespace SecureMessengerBohdan.Hubs
{
    [Authorize]
    public class ChatsHub : Hub<IChatsHubClient>
    {

    }
}

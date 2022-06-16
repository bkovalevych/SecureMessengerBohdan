using SecureMessengerBohdan.Application.Requests.GetChats;

namespace SecureMessengerBohdan.Hubs.Interfaces
{
    public interface IChatsHubClient
    {
        Task ChatCreated(GetChatDto getChatDto);
    }
}

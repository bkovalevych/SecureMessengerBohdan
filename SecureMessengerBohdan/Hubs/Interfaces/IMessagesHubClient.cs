using SecureMessengerBohdan.Application.Requests.GetMessages;

namespace SecureMessengerBohdan.Hubs.Interfaces
{
    public interface IMessagesHubClient
    {
        Task ReceiveMessage(GetMessageDto message);
    }
}

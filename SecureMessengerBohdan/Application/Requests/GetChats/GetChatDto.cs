using SecureMessengerBohdan.Application.Requests.GetMessages;

namespace SecureMessengerBohdan.Application.Requests.GetChats
{
    public class GetChatDto
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public GetMessageDto? LastMessage { get; set; }
    }
}

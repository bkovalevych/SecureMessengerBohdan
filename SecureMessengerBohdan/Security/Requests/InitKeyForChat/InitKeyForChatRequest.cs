using MediatR;

namespace SecureMessengerBohdan.Security.Requests.InitKeyForChat
{
    public class InitKeyForChatRequest : IRequest
    {
        public string ChatId { get; set; } 
    }
}

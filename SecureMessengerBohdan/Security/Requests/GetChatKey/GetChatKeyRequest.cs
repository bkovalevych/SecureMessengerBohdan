using MediatR;

namespace SecureMessengerBohdan.Security.Requests.GetChatKey
{
    public class GetChatKeyRequest : IRequest<GetChatKeyDto>
    {
        public string ChatId { get; set; }

        public string PublicRSAKey { get; set; }
    }
}

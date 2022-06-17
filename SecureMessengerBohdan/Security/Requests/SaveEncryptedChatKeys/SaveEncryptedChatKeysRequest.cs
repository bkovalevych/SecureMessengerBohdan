using MediatR;
using SecureMessengerBohdan.Security.Requests.GetChatKey;

namespace SecureMessengerBohdan.Security.Requests.CreateChatKeys
{
    public class SaveEncryptedChatKeysRequest : IRequest
    {
        public List<GetChatKeyDto> ChatKeys { get; set; } = new List<GetChatKeyDto>();
    }
}

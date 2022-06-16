using MediatR;
using SecureMessengerBohdan.Application.Requests.GetChats;

namespace SecureMessengerBohdan.Application.Requests.CreateChat
{
    public class CreateChatRequest : IRequest<GetChatDto>
    {
        public string ChatName { get; set; }

        public List<string> MemberIds { get; set; } = new List<string>();

    }
}

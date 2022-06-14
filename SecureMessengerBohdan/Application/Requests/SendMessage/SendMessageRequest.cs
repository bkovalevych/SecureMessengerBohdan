using MediatR;
using SecureMessengerBohdan.Application.Requests.GetMessages;

namespace SecureMessengerBohdan.Application.Requests.SendMessage
{
    public class SendMessageRequest : IRequest<GetMessageDto>
    {
        public string ChatId { get; set; }

        public string Text { get; set; }

    }
}

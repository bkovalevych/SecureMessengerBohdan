using MediatR;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.RequestHelpers.Interfaces;
using SecureMessengerBohdan.Application.Wrappers;

namespace SecureMessengerBohdan.Application.Requests.GetMessages
{
    public class GetMessagesRequest : IRequest<PagingList<GetMessageDto>>, IHasPaging
    {
        public string ChatId { get; set; }
        public Paging? Paging { get; set; }
    }
}
